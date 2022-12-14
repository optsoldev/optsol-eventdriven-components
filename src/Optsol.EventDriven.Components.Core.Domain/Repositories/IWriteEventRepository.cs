using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteEventRepository<in T> where T: IAggregate
{
    public int Commit(Guid correlationId, T entity);
    public void Rollback(T entity);
    public void Subscribe(params Action<IDomainEvent>[]? callback);
}