using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MediatR;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public class CriarBeneficiarioCommandHandler : IRequestHandler<CriarBeneficiarioCommand, Unit>
{
    private readonly IMessageBus _messageBus;
    private readonly IBeneficiarioWriteRepository _beneficiarioRepository;

    public CriarBeneficiarioCommandHandler(IMessageBus messageBus, IBeneficiarioWriteRepository beneficiarioRepository)
    {
        _messageBus = messageBus;
        _beneficiarioRepository = beneficiarioRepository;
    }
    
    public Task<Unit> Handle(CriarBeneficiarioCommand request, CancellationToken cancellationToken)
    {
        var beneficiario = new Beneficiario(request.PrimeiroNome, request.SegundoNome);

        if (beneficiario.IsInvalid)
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

