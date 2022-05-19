using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain;

public interface IMessageBus
{
    Task Publish(Guid integrationId, IEnumerable<IDomainEvent> events);
    Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events);
}