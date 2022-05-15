using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Driving.Projections
{
    public class Functions
    {
        private readonly IBeneficiarioAtualizadoWriteReadModelRepository _repository;

        public Functions(IBeneficiarioAtualizadoWriteReadModelRepository repository)
        {
            _repository = repository;
        }

        [FunctionName("Projection")]
        public void Run([QueueTrigger("beneficiarios-sucesso", Connection = "AzureWebJobsStorage")] IEvent @event, ILogger log)
        {
            _repository.ReceiveEvent(@event);
        }
    }
}
