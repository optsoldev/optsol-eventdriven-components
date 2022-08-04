using MassTransit;
using Sample.Hotel.Contracts.Commands;
using Sample.Hotel.Contracts.Events;

namespace Sample.Hotel.Driving.Commands.Consumers;

public class BookHotelConsumer : IConsumer<BookHotel>
{
    public async Task Consume(ConsumeContext<BookHotel> context)
    {
        Console.WriteLine("BookHotelConsumer {0}", context.Message.CorrelationId);
        Thread.Sleep(2000);

        if(context.Message.HotelId == 1)
        {
            var failed = new HotelBookedFailed
            {
                CorrelationId = context.Message.CorrelationId
            };

            await context.Publish(failed);
        }
        else
        {
            var hotelBooked = new HotelBooked
            {
                CorrelationId = context.Message.CorrelationId,
                TravelId = context.Message.TravelId,
            };

            await context.Publish(hotelBooked);
        }
    }
}
