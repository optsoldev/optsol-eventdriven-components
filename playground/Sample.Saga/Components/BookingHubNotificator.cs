using Optsol.EventDriven.Components.Driven.Infra.Notification.Hubs;

namespace Sample.Saga.Components;

public class BookingHubNotificator : HubNotificator, IBookingHubNotificator
{
    public BookingHubNotificator(string url) : base(url)
    {
    }
}