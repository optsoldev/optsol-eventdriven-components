using MediatR;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public record CriarBeneficiarioCommand(Guid CommandId, string PrimeiroNome, string SegundoNome) : IRequest<Unit>;