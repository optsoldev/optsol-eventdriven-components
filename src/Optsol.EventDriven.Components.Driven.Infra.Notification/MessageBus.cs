using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class MessageBus : IMessageBus
{
    public MessageBus()
    {
    }
    
    public async Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events)
    {
        //Sobe para o event hub
    }

    public async Task Publish(Guid integrationId, IEnumerable<IEvent> events)
    {
        //Sobe para o event hub
        
    }
}