
namespace Optsol.EventDriven.Components.Settings;

public enum MessageBusType
{
    RabbitMq,
    AzureServiceBus
};

public class MessageBusSettings
{
    public MessageBusType? MessageBusType { get; set; }
    public RabbitMqSettings? RabbitMqSettings { get; set; }
    public AzureServiceBusSettings? AzureServiceBusSettings { get; set; }
}