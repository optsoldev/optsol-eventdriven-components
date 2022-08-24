using Optsol.EventDriven.Component.Infra.Hubs;

namespace Sample.Saga.Components;

public class BookingHubNotificator : HubNotificator, IBookingHubNotificator
{
    public BookingHubNotificator(string url) : base(url)
    {
    }
}