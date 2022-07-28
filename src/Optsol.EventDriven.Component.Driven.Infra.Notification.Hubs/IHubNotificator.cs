namespace Optsol.EventDriven.Component.Driven.Infra.Notification.Hubs;

public interface IHubNotificator
{
    public Task NotifyAsync<T>(string method, T message);
}
