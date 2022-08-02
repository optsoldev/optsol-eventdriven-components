using Microsoft.Extensions.Configuration;
using Optsol.EventDriven.Components.Settings;

namespace Optsol.EventDriven.Components.MassTransit;

public class MessageBusUri
{
    private static MessageBusSettings? settings;

    public MessageBusUri(IConfiguration configuration)
    {
        settings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), settings);
    }

    public static Uri CreateUri(string name, ExchangeType exchangeType)
    {
        return new Uri(FormatAddress(name, exchangeType));
    }

    public static string FormatAddress(string name, ExchangeType exchangeType)
    {
        return $"{exchangeType.ToString(settings?.MessageBusType)}:{name.ToKebabCase()}";
    }
}