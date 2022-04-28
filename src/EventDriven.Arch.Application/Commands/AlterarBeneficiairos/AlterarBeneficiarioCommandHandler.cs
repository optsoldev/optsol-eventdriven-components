using EventDriven.Arch.Application.Commands.CriarBeneficiarios;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MediatR;

namespace EventDriven.Arch.Application.Commands.AlterarBeneficiairos;

public class AlterarBeneficiarioCommandHanlder : IRequestHandler<AlterarBeneficiarioCommand, Unit>
{
    private readonly IBeneficiarioWriteRepository _beneficiarioWriteRepository;
    private readonly IBeneficiarioReadRepository _beneficiarioReadRepository;
    
    public AlterarBeneficiarioCommandHanlder(IBeneficiarioWriteRepository beneficiarioRepository, IBeneficiarioReadRepository beneficiarioReadRepository)
    {
        _beneficiarioWriteRepository = beneficiarioRepository;
        _beneficiarioReadRepository = beneficiarioReadRepository;
    }
    
    public Task<Unit> Handle(AlterarBeneficiarioCommand request, CancellationToken cancellationToken)
    {
        var beneficiario = new Beneficiario(_beneficiarioReadRepository.GetById(request.beneficiarioId));

        beneficiario.AlterarNome(request.PrimeiroNome, request.SegundoNome);
        
        if (beneficiario.IsInvalid)
        {
            _beneficiarioWriteRepository.Rollback(request.IntegrationId, beneficiario);
        }
        else
        {
            _beneficiarioWriteRepository.Commit(request.IntegrationId, beneficiario);
        }

        return Task.FromResult(new Unit());
    }
}
