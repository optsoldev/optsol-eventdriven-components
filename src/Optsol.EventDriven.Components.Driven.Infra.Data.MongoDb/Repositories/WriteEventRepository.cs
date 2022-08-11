using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteEventRepository<T> : IWriteEventRepository<T> where T : IAggregate
{
    private readonly MongoContext context;
    private readonly INotificator notificator;

    private readonly IMongoCollection<PersistentEvent<IDomainEvent>> _set;
    protected WriteEventRepository(MongoContext context, string collectionName, INotificator notificator)
    {
        this.context = context;
        this.notificator = notificator;
        _set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
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
        
        context.AddTransaction(() => _set.InsertManyAsync(events));
        return context.SaveChanges();

    }

    public virtual void Rollback(T model)
    {
        model.Clear();
    }

    public virtual async Task Rollback<TFailedEvent>(T entity) where TFailedEvent : FailedEvent
    {
        Rollback(entity);

        var @event = new FailedEvent(entity.Id, entity.ValidationResult.Errors) as TFailedEvent;
        
        await notificator.Publish(@event);
    }

    public async Task<int> Commit<TSucessEvent>(Guid correlationId, T entity) where TSucessEvent : SuccessEvent
    {
        var result = Commit(correlationId, entity);
        if (result > 0)
        {
            var @event = new SuccessEvent(entity.Id, entity.Version) as TSucessEvent;
            
            await notificator.Publish(@event);
        }

        return result;
    }
}