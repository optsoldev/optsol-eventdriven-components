using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteRepository<in T> where T: IAggregate
{
    public void Commit(Guid correlationId, T entity);
    public void Rollback(T entity);
}