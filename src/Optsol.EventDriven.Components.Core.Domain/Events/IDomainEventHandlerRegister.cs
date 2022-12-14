using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Events;

public interface IDomainEventHandlerRegister
{
    public IList<Action<IDomainEvent>> Actions { get; } 
}