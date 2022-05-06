using EventDriven.Arch.Domain;
using MediatR;

namespace EventDriven.Arch.Application.Commands.Rollbacks;

public class RollbackCommandHandler : IRequestHandler<RollbackCommand, Unit>
{
    private readonly IBeneficiarioWriteRepository _beneficiarioWriteRepository;

    public RollbackCommandHandler(IBeneficiarioWriteRepository beneficiarioWriteRepository)
    {
        _beneficiarioWriteRepository = beneficiarioWriteRepository;
    }
    
    public Task<Unit> Handle(RollbackCommand request, CancellationToken cancellationToken)
    {
        _beneficiarioWriteRepository.RollbackIntegration(request.IntegrationId);

        return Task.FromResult(new Unit());
    }
}