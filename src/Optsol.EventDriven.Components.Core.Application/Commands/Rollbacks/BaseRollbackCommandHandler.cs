using EventDriven.Arch.Application.Commands.Rollbacks;
using MediatR;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Core.Application.Commands.Rollbacks;

public abstract class BaseRollbackCommandHandler<T> : IRequestHandler<RollbackCommand> where T : IAggregate
{
    private readonly IWriteRepository<T> _repository;

    public BaseRollbackCommandHandler(IWriteRepository<T> repository)
    {
        _repository = repository;
    }
    
    public Task<Unit> Handle(RollbackCommand request, CancellationToken cancellationToken)
    {
        _repository.RollbackIntegration();

        return Task.FromResult(new Unit());
    }
}