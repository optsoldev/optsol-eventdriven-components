namespace Optsol.EventDriven.Components.Infra.NotificationHub;

public interface IHubNotificator
{
    public Task NotifyAsync<T>(string method, T message);
}
