using MediatR;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Core.Domain;
using Sample.Flight.Contracts;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Core.Application.Commands;

public class BookFlightCommandHandler : BaseCommandHandler<FlightBook,BookFlightSucessEvent,BookFlightFailedEvent>, IRequestHandler<BookFlight, Unit>
{
    public BookFlightCommandHandler(ILogger<BookFlightCommandHandler> logger, 
        INotificator notificator,
        IFlightBookWriteRepository flightBookWriteRepository) : base(logger,flightBookWriteRepository, notificator)
    {
    }

    public async Task<Unit> Handle(BookFlight request, CancellationToken cancellationToken)
    {
        logger.LogDebug("BookFlight request {0}", request);

        var flightBook = FlightBook.Create(request.UserId, request.From, request.To);

        return await HandleValidation(request.CorrelationId, flightBook);
    }
}
