using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Events;

public interface IDomainEventHandler
{
    public void ReceiveEvent(IDomainEvent @event);
}