using MediatR;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Domain;
using Sample.Flight.Contracts;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Core.Application.Commands;

public class BookFlightCommandHandler : IRequestHandler<BookFlight, Unit>
{
    private readonly ILogger logger;
    private readonly INotificator notificator;
    private readonly IFlightBookWriteRepository flightBookWriteRepository;

    public BookFlightCommandHandler(ILogger<BookFlightCommandHandler> logger, 
        INotificator notificator,
        IFlightBookWriteRepository flightBookWriteRepository)
    {
        this.logger = logger;
        this.notificator = notificator;
        this.flightBookWriteRepository = flightBookWriteRepository;
    }

    public async Task<Unit> Handle(BookFlight request, CancellationToken cancellationToken)
    {
        logger.LogDebug("BookFlight request {0}", request);

        var flightBook = FlightBook.Create(request.UserId, request.From, request.To);

        if (flightBook.Invalid)
        {
            //rollback
            flightBookWriteRepository.Rollback(flightBook);
        }
        else
        {
            //commit
            flightBookWriteRepository.Commit(request.CorrelationId, flightBook);

            var flightBooked = new FlightBooked
            {
                CorrelationId = request.CorrelationId,
                TravelId = request.TravelId,
                ModelId = flightBook.Id,
                From = flightBook.From,
                To = flightBook.To
            };

            await notificator.Publish(flightBooked);
        }

        return new Unit();
    }
}
