using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public record CriarBeneficiarioCommand(string PrimeiroNome, string SegundoNome) : ICommand
{
    public Guid IntegrationId { get; init; } = Guid.NewGuid();
}