using Microsoft.AspNetCore.SignalR;

namespace Sample.Bff.Api.Hubs;

public class BookingNotificationHub : Hub
{
    public void TravelBooked(object message)
    {            
        Clients.All.SendAsync(nameof(TravelBooked), message);
    }


    public override async Task OnConnectedAsync()
    {
        Console.WriteLine(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        Console.WriteLine(Context.ConnectionId);
        await base.OnDisconnectedAsync(ex);
    }


}
