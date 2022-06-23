using MassTransit;
using Sample.Hotel.Commands;

namespace Sample.Hotel.Driving.Commands;

public class BookHotelConsumer : IConsumer<IBookHotel>
{
    public Task Consume(ConsumeContext<IBookHotel> context)
    {
        Console.WriteLine("BookHotelConsumer {0}", context.Message.CorrelationId);

        return Task.CompletedTask;
    }
}
