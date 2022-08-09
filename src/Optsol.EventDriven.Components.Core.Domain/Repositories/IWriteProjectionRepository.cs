using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteProjectionRepository<T> where T : IProjection
{
    public void ReceiveEvent(IDomainEvent @event);
}