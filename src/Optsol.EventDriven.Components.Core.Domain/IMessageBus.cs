namespace Optsol.EventDriven.Components.Core.Domain;

public interface IMessageBus
{
    Task Publish<T>(IEnumerable<T> events, string routingKey);
}