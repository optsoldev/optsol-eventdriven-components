using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MediatR;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public class CriarBeneficiarioCommandHandler : IRequestHandler<CriarBeneficiarioCommand, Unit>
{
    private readonly IBeneficiarioWriteRepository _beneficiarioRepository;

    public CriarBeneficiarioCommandHandler(IBeneficiarioWriteRepository beneficiarioRepository)
    {
        _beneficiarioRepository = beneficiarioRepository;
    }
    
    public Task<Unit> Handle(CriarBeneficiarioCommand request, CancellationToken cancellationToken)
    {
        var beneficiario = new Beneficiario(request.IntegrationId, request.PrimeiroNome, request.SegundoNome);

        if (beneficiario.Invalid)
        {
            _beneficiarioRepository.Rollback(request.IntegrationId, beneficiario);
        }
        else
        {
            _beneficiarioRepository.Commit(request.IntegrationId, beneficiario);
        }

        return Task.FromResult(new Unit());
    }
    
}

