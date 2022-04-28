using MediatR;

namespace EventDriven.Arch.Application.Commands.AlterarBeneficiairos;

public record AlterarBeneficiarioCommand(Guid IntegrationId, Guid beneficiarioId, string PrimeiroNome, string SegundoNome) : IRequest<Unit>;