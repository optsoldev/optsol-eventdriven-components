using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public record CriarBeneficiarioCommand(Guid IntegrationId, string PrimeiroNome, string SegundoNome) : ICommand
{
}