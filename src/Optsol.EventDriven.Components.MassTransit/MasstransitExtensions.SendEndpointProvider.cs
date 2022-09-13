using MassTransit;

namespace Optsol.EventDriven.Components.MassTransit;

public partial class MassTransitExtensions
{
    public static async  Task<ISendEndpoint> GetSendEndpoint<TCommand>(this ISendEndpointProvider sendEndpointProvider, TCommand command, ExchangeType exchangeType = ExchangeType.Queue)
    {
        var uri = MessageBusUri.GetInstance().CreateUri(command.GetType(), exchangeType);
        return await sendEndpointProvider.GetSendEndpoint(uri);
    }
    
    
}