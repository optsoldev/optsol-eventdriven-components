using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Flight.Contracts;

namespace Sample.Bff.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestClientSampleController : ControllerBase
{
    private readonly ILogger logger;
    private readonly IRequestClient<RequestClientSampleQuery> requestClient;

    public RequestClientSampleController(ILogger<RequestClientSampleController> logger,
        IRequestClient<RequestClientSampleQuery> requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        logger.LogInformation("RequestClientSample");
        
        var result = await requestClient.GetResponse<RequestClientSampleResult>(new {});

        return Ok(result.Message);
    }
}

