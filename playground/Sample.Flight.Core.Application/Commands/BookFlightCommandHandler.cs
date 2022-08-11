using MediatR;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Core.Domain;
using Sample.Flight.Contracts;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Core.Application.Commands;

public class BookFlightCommandHandler : BaseCommandHandler<FlightBook>, IRequestHandler<BookFlight, Unit>
{
    public BookFlightCommandHandler(ILogger<BookFlightCommandHandler> logger,
        IFlightBookWriteRepository flightBookWriteRepository) : base(logger,flightBookWriteRepository)
    {
    }

    public async Task<Unit> Handle(BookFlight request, CancellationToken cancellationToken)
    {
        logger.LogDebug("BookFlight request {0}", request);

        var flightBook = FlightBook.Create(request.UserId, request.From, request.To);

        await SaveChanges<BookFlightSucessEvent,BookFlightFailedEvent>(request.CorrelationId, flightBook);

        return new Unit();
    }
}
