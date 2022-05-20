using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteProjectionRepository<T> where T : IProjection
{
    public void ReceiveEvent(IDomainEvent @event);
}