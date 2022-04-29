namespace EventDriven.Arch.Domain;

public interface IMessageBus
{
    Task Publish(Guid integrationId, IEnumerable<IEvent> events);
    Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events);
}