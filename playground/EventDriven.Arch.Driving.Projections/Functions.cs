using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EventDriven.Arch.Driving.Projections;

public class Functions
{
    private readonly ILogger _logger;
    
    public Functions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Functions>();
       
        //CreateQueue();
    }

    [Function("Projecao")]
   public Task Run([RabbitMQTrigger(queueName: "saga-response-projection", ConnectionStringSetting = "rabbitMQConnectionAppSetting")] string req)
    {
        _logger.LogInformation($"{req}");

       return Task.CompletedTask;
   }

   
}
