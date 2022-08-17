using MassTransit;
using Sample.Saga.Contracts.Commands;

namespace Sample.Saga.Components;

public class BookingNotificationConsumer : IConsumer<BookingNotification>, IBookingNotificationConsumerAddress
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

