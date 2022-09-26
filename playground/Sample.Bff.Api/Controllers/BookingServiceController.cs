using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Saga.Contracts.Commands;
using Sample.Saga.Contracts.Events;

namespace Sample.Bff.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingServiceController : ControllerBase
{
    private readonly ILogger logger;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IRequestClient<TravelBookStatusRequested> requestClient;

    public BookingServiceController(ILogger<BookingServiceController> logger,
        IPublishEndpoint publishEndpoint,
        IRequestClient<TravelBookStatusRequested> requestClient)
    {
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
        this.requestClient = requestClient;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStatusAsync(Guid id)
    {
        logger.LogInformation("Travel Book Status {0}", id);

        var (status, notFound) = await requestClient.GetResponse<TravelBookStatus, TravelBookNotFound>(new { TravelId = id });

        if (status.IsCompletedSuccessfully)
        {
            var response = await status;
            return Ok(response.Message);
        }
        else
        {
            var response = await notFound;
            return NotFound(response.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(SubmitTravel submitTravel)
    {
        logger.LogInformation(message: "SubmitTravel for UserId {0}", submitTravel.UserId);

        var correlationId = await PublishSaga(submitTravel);

        return Accepted(new {CorrelationId = correlationId });
    }


    private async Task<Guid> PublishSaga(SubmitTravel submitTravel)
    {
        var correlationId = InVar.CorrelationId;

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
