using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteProjectionRepository<T> where T : IProjection, new()
{
    public void ReceiveEvent(IDomainEvent @event);
}