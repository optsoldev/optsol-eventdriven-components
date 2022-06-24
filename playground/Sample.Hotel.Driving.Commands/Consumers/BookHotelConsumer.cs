using MassTransit;
using Sample.Hotel.Contracts.Commands;
using Sample.Hotel.Contracts.Events;

namespace Sample.Hotel.Driving.Commands.Consumers;

public class BookHotelConsumer : IConsumer<BookHotel>
{
    public Task Consume(ConsumeContext<BookHotel> context)
    {
        Console.WriteLine("BookHotelConsumer {0}", context.Message.CorrelationId);

        var hotelBooked = new HotelBooked
        {
            CorrelationId = context.Message.CorrelationId,
            TravelId = context.Message.TravelId,
        };

        context.Publish(hotelBooked);

        return Task.CompletedTask;
    }
}
