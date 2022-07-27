using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteEventRepository<T> : IWriteEventRepository<T> where T : IAggregate
{
    private readonly MongoContext _context;

    private readonly List<Action<IDomainEvent>> _projectionCallbacks = new();
    private readonly IMongoCollection<PersistentEvent<IDomainEvent>> _set;
    protected WriteEventRepository(MongoContext context, string collectionName)
    {
        _context = context;
        _set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
    }

    public virtual void Commit(Guid correlationId, T model)
    {
        var events = model.PendingEvents.Select(e => new PersistentEvent<IDomainEvent>(
            correlationId,
            Guid.NewGuid(),
            model.Id,
            e.ModelVersion,
            e.When,
            e.GetType().AssemblyQualifiedName,
            e));
        
        _context.AddTransaction(() => _set.InsertManyAsync(events));
        _context.SaveChanges();

        foreach (var @event in model.PendingEvents)
        {
            foreach (var callback in _projectionCallbacks)
            {
                callback(@event);
            }
        }
    }

    public virtual void Rollback(T model)
    {
        model.Clear();
    }

    public void Subscribe(Action<IDomainEvent> callback)
    {
       _projectionCallbacks.Add(callback);
    }
}