namespace Optsol.EventDriven.Component.Infra.Hubs;

public interface IHubNotificator
{
    public Task NotifyAsync<T>(string method, T message);
}
