using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Events;

public class DomainEventHandlerRegister : IDomainEventHandlerRegister
{
    public DomainEventHandlerRegister(IEnumerable<IDomainEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            Actions.Add(handler.ReceiveEvent);
        }
    }

    public IList<Action<IDomainEvent>> Actions { get; } = new List<Action<IDomainEvent>>();
}