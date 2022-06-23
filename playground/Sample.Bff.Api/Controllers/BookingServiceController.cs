using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Saga.Contracts.Commands;
using Sample.Saga.Contracts.Events;

namespace Sample.Bff.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingServiceController : ControllerBase
{
    private readonly ILogger<BookingServiceController> _logger;
    private readonly IPublishEndpoint publishEndpoint;

    public BookingServiceController(ILogger<BookingServiceController> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitTravel submitTravel)
    {
        _logger.LogInformation(message: "SubmitTravel for UserId {0}", submitTravel.UserId);

        var correlationId = await PublishSaga(submitTravel);

        return Accepted(new {CorrelationId = correlationId });
    }


    private async Task<Guid> PublishSaga(SubmitTravel submitTravel)
    {
        var correlationId = Guid.NewGuid();

        await publishEndpoint.Publish<ITravelBookingSubmitted>(new
        {
            TravelId = Guid.NewGuid(),
            CorrelationId = correlationId,
            submitTravel.UserId,
            submitTravel.From,
            submitTravel.To,
            submitTravel.HotelId,
            submitTravel.Departure,
        });

        return correlationId;
    }
}
