using Optsol.EventDriven.Components.Infra.NotificationHub;

namespace Sample.Saga.Components;

public class BookingHubNotificator : HubNotificator, IBookingHubNotificator
{
    public BookingHubNotificator(string url) : base(url)
    {
    }
}