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

    public static Uri GetUri(string name, ExchangeType exchangeType)
    {
        var nameExchangeType = exchangeType == ExchangeType.Exchange ? settings?.MessageBusType == MessageBusType.RabbitMq ? "exchange" : "topic" : "queue";
        return new Uri($"{nameExchangeType}:{name}");
    }
}