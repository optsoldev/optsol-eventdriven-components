using MediatR;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Domain;
using Sample.Flight.Contracts.Commands;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Core.Application.Commands
{
    public class UnbookFlightCommandHandler : IRequestHandler<UnbookFlight, Unit>
    {
        private readonly ILogger<UnbookFlightCommandHandler> logger;
        private readonly INotificator notificator;
        private readonly IFlightBookWriteRepository flightBookWriteRepository;
        private readonly IFlightBookReadRepository flightBookReadRepository;

        public UnbookFlightCommandHandler(ILogger<UnbookFlightCommandHandler> logger, INotificator notificator, IFlightBookWriteRepository flightBookWriteRepository, IFlightBookReadRepository flightBookReadRepository)
        {
            this.logger = logger;
            this.notificator = notificator;
            this.flightBookWriteRepository = flightBookWriteRepository;
            this.flightBookReadRepository = flightBookReadRepository;
        }

        public Task<Unit> Handle(UnbookFlight request, CancellationToken cancellationToken)
        {
            logger.LogDebug("BookFlight request {0}", request);

            var flightBook = new FlightBook(flightBookReadRepository.GetById(request.ModelId));

            flightBook.Unbook();

            if (flightBook.Invalid)
            {
                //rollback
                flightBookWriteRepository.Rollback(flightBook);
            }
            else
            {
                //commit
                flightBookWriteRepository.Commit(request.CorrelationId, flightBook);                
            }

            return Task.FromResult(new Unit());
        }
    }
}
