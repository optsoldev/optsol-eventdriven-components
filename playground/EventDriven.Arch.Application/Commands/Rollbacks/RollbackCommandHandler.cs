using EventDriven.Arch.Application.Commands.Commits;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Application.Commands.Rollbacks;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace EventDriven.Arch.Application.Commands.Rollbacks;

public class RollbackCommandHandler : BaseRollbackCommandHandler<Beneficiario>
{
    public RollbackCommandHandler(IWriteRepository<Beneficiario> repository) : base(repository)
    {
    }
}