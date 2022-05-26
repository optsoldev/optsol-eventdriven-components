using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EventDriven.Arch.Driving.Projections
{
    public class Functions
    {
        [FunctionName("Functions")]
        public async Task Run([EventHubTrigger("beneficiario-saga-response", Connection = "ConnectionString")] EventData @event, ILogger log)
        {
            log.LogInformation($"{@event}");
        }
    }
}
