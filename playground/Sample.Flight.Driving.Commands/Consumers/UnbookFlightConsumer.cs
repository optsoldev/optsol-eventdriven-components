using MassTransit;
using MediatR;
using Optsol.EventDriven.Components.Core.Contracts;
using Optsol.EventDriven.Components.MassTransit;
using Sample.Flight.Contracts;

namespace Sample.Flight.Driving.Commands.Consumers
{
 
    public class UnbookFlightConsumer : IConsumer<UnbookFlight>, IUnbookFlightConsumerAddress
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UnbookFlightConsumer> _logger;

        public UnbookFlightConsumer(ILogger<UnbookFlightConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UnbookFlight> context)
        {
            _logger.LogDebug(message: "UnbookFlightConsumer {0}", args: context.Message.CorrelationId);

            await _mediator.Send(context.Message);
        }
    }
}
