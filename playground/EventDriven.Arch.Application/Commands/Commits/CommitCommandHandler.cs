using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Application.Commands.Commits;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace EventDriven.Arch.Application.Commands.Commits;

public class CommitCommandHandler : BaseCommitCommandHandler<Beneficiario>
{
    public CommitCommandHandler(IWriteRepository<Beneficiario> repository) : base(repository)
    {
    }
}