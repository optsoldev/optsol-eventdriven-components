using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteRepository<in T> where T: IAggregate
{
    public void RollbackIntegration(Guid integrationId);
    public void CommitIntegration(Guid integrationId);
    public void Commit(Guid integrationId, T entity);
    public void Rollback(Guid integrationId, T entity);
}