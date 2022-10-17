using MassTransit;

namespace Optsol.EventDriven.Components.MassTransit;

public partial class MassTransitExtensions
{
    public static async Task<ISendEndpoint> GetSendEndpoint<TCommand>(this ISendEndpointProvider sendEndpointProvider,
        TCommand command, ExchangeType exchangeType = ExchangeType.Queue) where TCommand : class
    {
        var uri = MessageBusUri.GetInstance().CreateUri(command!.GetType(), exchangeType);
        return await sendEndpointProvider.GetSendEndpoint(uri);
    }

    public static async Task Execute<TCommand>(this ISendEndpointProvider sendEndpointProvider, TCommand command,
        ExchangeType exchangeType = ExchangeType.Queue) where TCommand : class
    {
        var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(command, exchangeType);

        await sendEndpoint.Send(command);
    }
}