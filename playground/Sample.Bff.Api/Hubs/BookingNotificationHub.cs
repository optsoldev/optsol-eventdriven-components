using Microsoft.AspNetCore.SignalR;

namespace Sample.Bff.Api.Hubs
{
    public class BookingNotificationHub : Hub
    {
        public void FlightBooked(object message)
        {            
            Clients.All.SendAsync("FlightBooked", message);
        }

        public void CarBooked(object message)
        {
            Clients.All.SendAsync("CarBooked", message);
        }

        public void HotelBooked(object message)
        {
            Clients.All.SendAsync("HotelBooked", message);
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
}
