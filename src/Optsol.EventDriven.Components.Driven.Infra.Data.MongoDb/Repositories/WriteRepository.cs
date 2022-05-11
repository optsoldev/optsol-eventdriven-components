using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteRepository<T> : IWriteRepository<T> where T : IAggregate
{
    private readonly MongoContext _context;
    private readonly IMessageBus _messageBus;
    private readonly IMongoCollection<PersistentEvent<IEvent>> _set;
    private readonly IMongoCollection<StagingEvent<IEvent>> _setStaging;
    private readonly List<Action<IEvent>> _projectionCallbacks = new();
    protected WriteRepository(MongoContext context, IMessageBus messageBus, string collectionName)
    {
        _context = context;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent<IEvent>>(collectionName);
        _setStaging = context.GetCollection<StagingEvent<IEvent>>($"{collectionName}-Staging");
    }
    
    public virtual void RollbackIntegration(Guid integrationId)
    {
        _context.AddTransaction(() => _setStaging.DeleteManyAsync(f => f.IntegrationId == integrationId));
        _context.SaveChanges();
    }

    public virtual void CommitIntegration(Guid integrationId)
    {
        var sortDef = Builders<StagingEvent<IEvent>>.Sort.Descending(d => d.ModelVersion);

        var events = _setStaging
            .Find(e => e.IntegrationId == integrationId)
            .Sort(sortDef)
            .Project(p => (PersistentEvent<IEvent>)p)
            .ToList();

        _context.AddTransaction(() => _set.InsertManyAsync(events));
        _context.AddTransaction(() => _setStaging.DeleteManyAsync(f => f.IntegrationId == integrationId));
        _context.SaveChanges();

        SendEvents(events.Select(e => e.Data));
    }

    private void SendEvents(IEnumerable<IEvent> events)
    {
        foreach (var @event in events)
        {
            foreach (var callback in _projectionCallbacks)
            {
                callback(@event);
            }
        }
    }
    
    public virtual void Commit(Guid integrationId, T model)
    {
        var events = model.PendingEvents.Select(e => new StagingEvent<IEvent>(
            integrationId,
            model.Id,
            e.ModelVersion,
            e.When,
            e.GetType().AssemblyQualifiedName,
            e));
        
        _context.AddTransaction(() => _setStaging.InsertManyAsync(events));
        _context.SaveChanges();
        _messageBus.Publish(integrationId, model.PendingEvents);
        SendEvents(events.Select(e => e.Data));
    }

    public virtual void Rollback(Guid integrationId, T model)
    {
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }

    public virtual void Subscribe(Action<IEvent> callback)
    {
        _projectionCallbacks.Add(callback);
    }
}