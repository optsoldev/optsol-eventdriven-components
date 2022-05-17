using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EventDriven.Arch.Driving.Projections
{
    public class Functions
    {
        [FunctionName("Functions")]
<<<<<<< HEAD
        public async Task Run([EventHubTrigger("beneficiarios-resposta", Connection = "ConnectionString")] EventData @event, ILogger log)
=======
        public async Task Run([EventHubTrigger("beneficiario-success", Connection = "ConnectionString")] EventData @event, ILogger log)
>>>>>>> 53426e8e4cc66e8e5b39c2f80ad296bf56cd321a
        {
            log.LogInformation($"{@event}");
        }
    }
}
