using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain;

public interface IMessageBus
{
    Task Publish(IEnumerable<IDomainEvent> events);
    Task Publish(IEnumerable<IFailureEvent> events);
}