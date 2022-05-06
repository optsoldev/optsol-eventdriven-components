using EventDriven.Arch.Domain;
using MediatR;

namespace EventDriven.Arch.Application.Commands.Commits;

public class CommitCommandHandler :IRequestHandler<CommitCommand, Unit>
{
    private readonly IBeneficiarioWriteRepository _repository;

    public CommitCommandHandler(IBeneficiarioWriteRepository repository)
    {
        _repository = repository;
    }
    
    public Task<Unit> Handle(CommitCommand request, CancellationToken cancellationToken)
    {
        _repository.CommitIntegration(request.IntegrationId);

        return Task.FromResult(new Unit());
    }
}