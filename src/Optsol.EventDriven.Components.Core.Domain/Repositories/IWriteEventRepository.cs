using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteEventRepository<in T> where T: IAggregate
{
    public void Commit(Guid correlationId, T entity);
    public void Rollback(T entity);
    public void Subscribe(params Action<IDomainEvent>[] callback);
}