using Microsoft.Extensions.Configuration;
using Optsol.EventDriven.Components.Core.Contracts;
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
        if (!uriName.GetInterfaces().Any(x => typeof(IConsumerAddress).IsAssignableFrom(uriName)))
        {
            throw new ArgumentException($"Type {uriName} must be assignable to {nameof(IConsumerAddress)}");
        }
        
        return new Uri(FormatAddress(uriName.Name, exchangeType));
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
        name = name.Replace("I", "").Replace("ConsumerAddress", "").ToKebabCase();
        return string.IsNullOrWhiteSpace(settings?.Prefix) ? name : $"{settings.Prefix}-{name}";
    }
}