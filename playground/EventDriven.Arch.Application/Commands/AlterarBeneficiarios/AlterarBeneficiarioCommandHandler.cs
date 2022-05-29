using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using MediatR;

namespace EventDriven.Arch.Application.Commands.AlterarBeneficiarios;

public class AlterarBeneficiarioCommandHandler : IRequestHandler<AlterarBeneficiarioCommand, Unit>
{
    private readonly IBeneficiarioWriteRepository _beneficiarioWriteRepository;
    private readonly IBeneficiarioReadRepository _beneficiarioReadRepository;
    
    public AlterarBeneficiarioCommandHandler(IBeneficiarioWriteRepository beneficiarioRepository, IBeneficiarioReadRepository beneficiarioReadRepository)
    {
        _beneficiarioWriteRepository = beneficiarioRepository;
        _beneficiarioReadRepository = beneficiarioReadRepository;
    }
    
    public Task<Unit> Handle(AlterarBeneficiarioCommand request, CancellationToken cancellationToken)
    {
        var beneficiario = new Beneficiario(_beneficiarioReadRepository.GetById(request.BeneficiarioId));

        beneficiario.AlterarNome(request.PrimeiroNome, request.SegundoNome);
        
        if (beneficiario.Invalid)
        {
            _beneficiarioWriteRepository.Rollback(beneficiario);
        }
        else
        {
            _beneficiarioWriteRepository.Commit(beneficiario);
        }

        return Task.FromResult(new Unit());
    }
}
