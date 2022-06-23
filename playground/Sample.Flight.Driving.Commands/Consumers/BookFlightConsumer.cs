﻿using MassTransit;
using MediatR;
using Sample.Flight.Contracts.Commands;
using Sample.Flight.Contracts.Events;

namespace Sample.Flight.Driving.Commands.Consumers;

public class BookFlightConsumer : IConsumer<BookFlight>
{
    private readonly IMediator _mediator;
    private readonly ILogger<BookFlightConsumer> _logger;

    public BookFlightConsumer(ILogger<BookFlightConsumer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<BookFlight> context)
    {

        _logger.LogDebug("BookFlightConsumer {0}", context.Message.CorrelationId);

        await _mediator.Send(context.Message);

        await context.Publish<IFlightBooked>(new
        {
            context.Message.CorrelationId,
            context.Message.TravelId
        });
    }
}
