using MassTransit;
using Sample.Hotel.Contracts.Commands;
using Sample.Hotel.Contracts.Events;

namespace Sample.Hotel.Driving.Commands.Consumers;

public class BookHotelConsumer : IConsumer<BookHotel>, IBookHotelConsumerAddress
{
    public async Task Consume(ConsumeContext<BookHotel> context)
    {
        Console.WriteLine("BookHotelConsumer {0}", context.Message.CorrelationId);

        await Task.Delay(1000);
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


 public class BookHotelConsumerDefinition :
    ConsumerDefinition<BookHotelConsumer>
{
    public BookHotelConsumerDefinition()
    {
        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 4;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<BookHotelConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}
