using MediatR;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Core.Domain;
using Sample.Flight.Contracts;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Core.Application.Commands;

public class BookFlightCommandHandler : BaseCommandHandler<FlightBook>, IRequestHandler<BookFlight, Unit>
{
    private readonly INotificator notificator;
    
    public BookFlightCommandHandler(ILogger<BookFlightCommandHandler> logger,
        IFlightBookWriteRepository flightBookWriteRepository, INotificator notificator) : base(logger,flightBookWriteRepository)
    {
        this.notificator = notificator;
    }

    public async Task<Unit> Handle(BookFlight request, CancellationToken cancellationToken)
    {
        logger.LogDebug("BookFlight request {0}", request);

        var flightBook = FlightBook.Create(request.UserId, request.From, request.To);

        if (await SaveChanges(request.CorrelationId, flightBook))
        {
            var @event = new BookFlightSucessEvent(flightBook.Id, flightBook.Version, flightBook.UserId);
            await notificator.Publish(@event);
        }
        else
        {
            var @event = new BookFlightFailedEvent(flightBook.Id, flightBook.ValidationResult.Errors, flightBook.UserId);
            await notificator.Publish(@event);
        }

        return new Unit();
    }
}
