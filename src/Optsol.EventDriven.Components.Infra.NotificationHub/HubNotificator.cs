﻿using Microsoft.AspNetCore.SignalR.Client;

namespace Optsol.EventDriven.Components.Infra.NotificationHub;

public abstract class HubNotificator : IHubNotificator
{
    private readonly HubConnection hubConnection;

    public HubNotificator(string url)
    {
        this.hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        };
        
    }
    public async Task NotifyAsync<T>(string method, T message)
    {
        if (this.hubConnection.State == HubConnectionState.Disconnected)
            await this.hubConnection.StartAsync();

        await this.hubConnection.InvokeAsync(method, message);
    }
}