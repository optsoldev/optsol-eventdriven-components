using MassTransit;
using Sample.Flight.Contracts.Commands;
using Sample.Flight.Contracts.Events;

namespace Sample.Flight.Driving.Commands.Consumers;

public class BookFlightConsumer : IConsumer<IBookFlight>
{
    private readonly ILogger<BookFlightConsumer> _logger;

    public BookFlightConsumer(ILogger<BookFlightConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<IBookFlight> context)
    {

        Console.WriteLine("BookFlightConsumer {0}", context.Message.CorrelationId);

        return context.Publish<IFlightBooked>(new
        {
            context.Message.CorrelationId,
            context.Message.TravelId
        });
    }
}
