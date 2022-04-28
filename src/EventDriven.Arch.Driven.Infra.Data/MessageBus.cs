using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;

namespace EventDriven.Arch.Driven.Infra.Data;

public class MessageBus : IMessageBus
{
    private readonly IDomainHub _hub;

    public MessageBus(IDomainHub hub)
    {
        _hub = hub;
    }
    
    public async Task Publish(Guid integrationId, IEnumerable<IIntegrationFailureEvent> events)
    {
        //Sobe para o event hub
        
        foreach (var evt in events)
        {
            await _hub.BroadcastFailure(integrationId, evt);
        }
    }

    public async Task Publish(Guid integrationId, IEnumerable<IIntegrationSucessEvent> events)
    {
        //Sobe para o event hub
        
        foreach (var evt in events)
        {
            await _hub.BroadcastSuccess(integrationId, evt);
        }
    }
}