using EventDriven.Arch.Application.Commands.CriarBeneficiarios;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EventDriven.Arch.Driving.Commands
{
    public class Functions
    {
        private readonly IMediator _mediator;
        //private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public Functions(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <exception cref="InvalidCastException"></exception>
        [Function("CriarBeneficiario")]
        public async Task CriarBeneficiarioAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/beneficiarios/criar")] HttpRequestData req)
        {
            //_logger.LogInformation("Criar Beneficiário Triggered");

            var data = await JsonSerializer.DeserializeAsync<CriarBeneficiarioCommand>(req.Body);

            if (data == null) throw new InvalidCastException($"Não foi possível converter para {nameof(CriarBeneficiarioCommand)}");

            await _mediator.Send(data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Function("CriarDependente")]
        public Task CriarDependente([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "v1/beneficiarios/{id}/dependentes/criar")] HttpRequestData req,
            ILogger log)
        {
            log.LogInformation("Enviar Envelope Triggered");

            //var data = await JsonSerializer.DeserializeAsync<CriarDependenteCommand>(req.Body, _jsonSerializerOptions);

            //if (data == null) throw new InvalidCastException("Não foi possível converter para Envelope");
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Function("AdicionarEndereco")]
        public Task AdicionarEndereco(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/beneficiarios/{id}/enderecos/adicionar")] HttpRequestData req,
            ILogger log)
        {
            //log.LogInformation("Enviar Envelope Triggered");

            //var data = await JsonSerializer.DeserializeAsync<EnvelopeViewModel>(req.Body, _jsonSerializerOptions);

            //if (data == null) throw new InvalidCastException("Não foi possível converter para Envelope");
            throw new NotImplementedException();
        }


    }
}
