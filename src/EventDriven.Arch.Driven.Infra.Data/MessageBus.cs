using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Driven.Infra.Data;

public class MessageBus : IMessageBus
{
    private readonly IDomainHub _hub;

    public MessageBus(IDomainHub hub)
    {
        _hub = hub;
    }
    
    public async Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events)
    {
        //Sobe para o event hub
        
        foreach (var evt in events)
        {
            await _hub.BroadcastFailure(integrationId, evt);
        }
    }

    public async Task Publish(Guid integrationId, IEnumerable<IEvent> events)
    {
        //Sobe para o event hub
        
        foreach (var evt in events)
        {
            await _hub.BroadcastSuccess(integrationId, evt);
        }
    }
}