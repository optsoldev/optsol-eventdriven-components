using Microsoft.AspNetCore.SignalR.Client;

namespace Optsol.EventDriven.Component.Infra.Hubs;

public abstract class HubNotificator : IHubNotificator
{
    private readonly HubConnection hubConnection;

    public HubNotificator(string url)
    {
        this.hubConnection = new HubConnectionBuilder()
                                .WithUrl(url)
                                .WithAutomaticReconnect()
                                .Build();


    }
    public async Task NotifyAsync<T>(string method, T message)
    {
        if (this.hubConnection.State == HubConnectionState.Disconnected)
            await this.hubConnection.StartAsync();

        await this.hubConnection.InvokeAsync(method, message);
    }
}
