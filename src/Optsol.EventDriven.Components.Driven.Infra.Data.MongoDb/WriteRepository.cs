using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

public abstract class WriteRepository<T> : IWriteRepository<T> where T : IAggregate
{
    protected readonly MongoContext _context;
    protected readonly IMessageBus _messageBus;
    protected readonly IMongoCollection<PersistentEvent<IEvent>> _set;
    protected readonly IMongoCollection<StagingEvent<IEvent>> _setStaging;
    
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
    }

    public virtual void Rollback(Guid integrationId, T model)
    {
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }
}