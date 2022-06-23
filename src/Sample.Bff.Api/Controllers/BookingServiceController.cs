using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Saga.Contracts;

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
    public async Task<IActionResult> Post(Guid userId)
    {
        _logger.LogInformation(message: "SubmitTravel for UserId {0}", userId);

        var correlationId = await PublishSaga(userId);

        return Accepted(new {CorrelationId = correlationId });
    }

    private async Task<Guid> PublishSaga(Guid userId)
    {
        var correlationId = Guid.NewGuid();

        await publishEndpoint.Publish<ITravelBookingSubmitted>(new
        {
            TravelId = Guid.NewGuid(),
            CorrelationId = correlationId,
            UserId = userId
        });

        return correlationId;
    }
}
