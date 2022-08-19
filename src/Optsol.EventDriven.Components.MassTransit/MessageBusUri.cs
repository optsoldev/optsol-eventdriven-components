using Optsol.EventDriven.Components.Settings;

namespace Optsol.EventDriven.Components.MassTransit;

public class MessageBusUri
{
    private static MessageBusUri? messageBusUri;
    
    private readonly MessageBusSettings? settings;
    
    public MessageBusUri(MessageBusSettings settings)
    {
        this.settings = settings;
        messageBusUri ??= this;
    }

    public static MessageBusUri GetInstance()
    {
        ArgumentNullException.ThrowIfNull(messageBusUri, "MessageBusUri");
        
        return messageBusUri;
    }
    
    public Uri CreateUri(Type uriName, ExchangeType exchangeType = ExchangeType.Queue)
    {
        return new Uri(FormatAddress(uriName.Name, exchangeType));
    }
    
    public Uri CreateUri(string uriName, ExchangeType exchangeType = ExchangeType.Queue)
    {
        return new Uri(FormatAddress(uriName, exchangeType));
    }

    /// <summary>
    /// Format name and change exchange to topic if is using AzureServiceBus.
    /// </summary>
    /// <param name="name">string with the name of the uri to be formated</param>
    /// <param name="exchangeType">ExchangeType</param>
    /// <returns></returns>
    private string FormatAddress(string name, ExchangeType exchangeType)
    {
        name = FormatName(name);
        return $"{exchangeType.ToString(settings?.MessageBusType)}:{name}";
    }

    /// <summary>
    /// Format name removing I and ConsumerAddress from Interface name and add KebabCase.
    /// </summary>
    /// <param name="name">string to be formated</param>
    /// <returns>name formated.</returns>
    public string FormatName(string name)
    {
        name = string.IsNullOrWhiteSpace(settings?.Prefix) ? name.ToKebabCase() : $"{settings.Prefix}-{name}".ToKebabCase();

        return name.Replace("-command","").Replace("-query", "").Replace("-event", "");
        
    }
}