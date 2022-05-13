using MediatR;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Core.Application.Commands.Commits;

public abstract class BaseCommitCommandHandler<T> : IRequestHandler<CommitCommand> where T : IAggregate
{
    private readonly IWriteRepository<T> _repository;

    public BaseCommitCommandHandler(IWriteRepository<T> repository)
    {
        _repository = repository;
    }
    
    public Task<Unit> Handle(CommitCommand request, CancellationToken cancellationToken)
    {
        _repository.CommitIntegration(request.IntegrationId);

        return Task.FromResult(new Unit());
    }
}