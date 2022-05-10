using EventDriven.Arch.Application.Commands.Commits;
using EventDriven.Arch.Domain;
using MediatR;

namespace EventDriven.Arch.Application.Commands.Rollbacks;

public class RollbackCommandHandler : IRequestHandler<RollbackCommand>
{
    private readonly IBeneficiarioWriteRepository _repository;

    public RollbackCommandHandler(IBeneficiarioWriteRepository repository)
    {
        _repository = repository;
    }
    
    public Task<Unit> Handle(RollbackCommand request, CancellationToken cancellationToken)
    {
        _repository.RollbackIntegration(request.IntegrationId);

        return Task.FromResult(new Unit());
    }
}