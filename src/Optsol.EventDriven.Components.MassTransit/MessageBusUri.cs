using Microsoft.Extensions.Configuration;
using Optsol.EventDriven.Components.Settings;

public enum ExchangeType
{
    Queue,
    Exchange
}


public class MessageBusUri
{
    private static MessageBusSettings? settings { get; set; }

    public MessageBusUri(IConfiguration configuration)
    {
        settings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), settings);
    }

    public static Uri CreateUri(string name, ExchangeType exchangeType)
    {
        return new Uri($"{exchangeType.ToString(settings?.MessageBusType)}:{name.ToKebabCase()}");
    }
}