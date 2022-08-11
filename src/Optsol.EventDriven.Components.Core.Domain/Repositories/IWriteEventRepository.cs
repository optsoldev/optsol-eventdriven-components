using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteEventRepository<in T> where T: IAggregate
{
    public int Commit(Guid correlationId, T entity);
    public void Rollback(T entity);
    public Task Rollback<TFailedEvent>(T entity) where TFailedEvent : FailedEvent;
    public Task<int> Commit<TSucessEvent>(Guid correlationId, T entity) where TSucessEvent : SuccessEvent;
    public void Subscribe(params Action<IDomainEvent>[]? callback);
}