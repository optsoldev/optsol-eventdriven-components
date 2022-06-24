using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
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
        var beneficiario = Beneficiario.Create(request.PrimeiroNome, request.SegundoNome);

        if (beneficiario.Invalid)
        {
            _beneficiarioRepository.Rollback(beneficiario);
        }
        else
        {
            _beneficiarioRepository.Commit(request.CorrelationId, beneficiario);
        }

        return Task.FromResult(new Unit());
    }
    
}

