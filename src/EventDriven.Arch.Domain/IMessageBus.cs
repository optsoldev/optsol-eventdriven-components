using EventDriven.Arch.Domain.Beneficiarios;

namespace EventDriven.Arch.Domain;

public interface IMessageBus
{
    Task Publish(Guid integrationId, IEnumerable<IIntegrationFailureEvent> events);
    Task Publish(Guid integrationId, IEnumerable<IIntegrationSucessEvent> events);
}