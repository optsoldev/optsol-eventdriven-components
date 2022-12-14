using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteEventRepository<T> : IWriteEventRepository<T> where T : IAggregate
{
    private readonly MongoContext context;

    private readonly List<Action<IDomainEvent>> projectionCallbacks = new();
    private readonly IMongoCollection<PersistentEvent<IDomainEvent>> set;
    protected WriteEventRepository(MongoContext context, string collectionName, IDomainEventHandlerRegister? registered = null)
    {
        Subscribe(registered?.Actions.ToArray());
        this.context = context;
        set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
    }

    public virtual int Commit(Guid correlationId, T model)
    {
        var events = model.PendingEvents.Select(e => new PersistentEvent<IDomainEvent>(
            correlationId,
            Guid.NewGuid(),
            model.Id,
            e.ModelVersion,
            e.When,
            e.GetType().AssemblyQualifiedName,
            e));
        
        context.AddTransaction(() => set.InsertManyAsync(events));
        
        foreach (var @event in model.PendingEvents)
        {
            foreach (var callback in projectionCallbacks)
            {
                callback(@event);
            }
        }
        
        return context.SaveChanges();
    }

    public virtual void Rollback(T model)
    {
        model.Clear();
    }
    
    public void Subscribe(params Action<IDomainEvent>[]? callback)
    {
        if(callback is not null)
            projectionCallbacks.AddRange(callback);
    }
}