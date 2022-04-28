using MediatR;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public record CriarBeneficiarioCommand(Guid IntegrationId, string PrimeiroNome, string SegundoNome) : IRequest<Unit>;