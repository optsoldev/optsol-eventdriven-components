using EventDriven.Arch.Application.Commands.AlterarBeneficiairos;
using EventDriven.Arch.Application.Commands.CriarBeneficiarios;
using EventDriven.Arch.Application.Queries.BuscarBeneficiario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventDriven.Arch.Driving.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BeneficiariosController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CriarBeneficiario(CriarBeneficiarioCommand command)
        {
            await _mediator.Send(command);

            return Ok(command.IntegrationId.ToString());
        }

        [HttpPost("id")]
        public async Task<IActionResult> AlterarBeneficiario(AlterarBeneficiarioCommand command)
        {
            await _mediator.Send(command);

            return Ok(command.IntegrationId.ToString());
        }

        [HttpGet("id")]
        public async Task<IActionResult> BuscarBeneficiario(BuscarBeneficiarioQuery query)
        {
            var beneficiario = await _mediator.Send(query);
            return Ok(beneficiario);
        }
    }
}


