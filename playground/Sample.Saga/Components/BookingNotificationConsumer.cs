using MassTransit;

namespace Sample.Saga.Components;

public class BookingNotificationConsumer : IConsumer<BookingNotification>
{
    private readonly IBookingHubNotificator hubNotificator;

    public BookingNotificationConsumer(IBookingHubNotificator hubNotificator)
    {
        this.hubNotificator = hubNotificator;
    }

    public async Task Consume(ConsumeContext<BookingNotification> context)
    {
        await hubNotificator.NotifyAsync("TravelBooked", context.Message);
    }
}

