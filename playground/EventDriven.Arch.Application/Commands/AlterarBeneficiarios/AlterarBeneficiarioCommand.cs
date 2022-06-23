using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Commands.AlterarBeneficiarios;

public record AlterarBeneficiarioCommand(Guid CorrelationId, Guid BeneficiarioId, string PrimeiroNome, string SegundoNome) : ICommand
{
}