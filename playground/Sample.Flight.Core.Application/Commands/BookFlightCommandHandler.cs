using MediatR;
using Microsoft.Extensions.Logging;
using Sample.Flight.Contracts.Commands;

namespace Sample.Flight.Core.Application.Commands;

public class BookFlightCommandHandler : IRequestHandler<BookFlight, Unit>
{
    private readonly ILogger _logger;
    public BookFlightCommandHandler(ILogger<BookFlightCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<Unit> Handle(BookFlight request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("BookFlight request {0}", request);
        return Task.FromResult(new Unit());
    }
}
