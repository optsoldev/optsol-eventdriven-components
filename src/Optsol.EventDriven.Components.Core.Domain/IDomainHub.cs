namespace Optsol.EventDriven.Components.Core.Domain;

public interface IDomainHub
{
    public Task BroadcastSuccess(Guid integrationId, IEvent @event);

    public Task BroadcastFailure(Guid integrationId, IFailureEvent @event);
}