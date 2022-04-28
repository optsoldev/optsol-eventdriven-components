using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventDriven.Arch.Application.Commands.AlterarBeneficiairos;
using EventDriven.Arch.Application.Commands.CriarBeneficiarios;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventDrive.Arch.Driving.Api.Controllers
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
        public async Task<IActionResult> CriarBeneficiario(CriarBeneficiarioViewModel viewModel)
        {
            var command = new CriarBeneficiarioCommand(Guid.NewGuid(), viewModel.PrimeiroNome, viewModel.SegundoNome);

            await _mediator.Send(command);

            return Ok(command.IntegrationId.ToString());
        }

        [HttpPost("id")]
        public async Task<IActionResult> AlterarBeneficiario(AlterarBeneficiarioViewModel viewModel)
        {
            var command = new AlterarBeneficiarioCommand(Guid.NewGuid(), Guid.Parse(viewModel.Id), viewModel.PrimeiroNome,
                viewModel.SegundoNome);

            await _mediator.Send(command);

            return Ok(command.IntegrationId.ToString());
        }

        [HttpGet("id")]
        public async Task<IActionResult> BuscarBeneficiario(string id)
        {
            var beneficiarioId = Guid.Parse(id);

            return Ok();
        }
    }
}


