using MassTransit;
using Optsol.EventDriven.Components.Core.Domain;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class Notificator : INotificator
{
    private IPublishEndpoint _endpoint;

    public Notificator(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }

    public async Task Publish<T>(T @event)
    {
        await _endpoint.Publish(@event);        
    }    
}