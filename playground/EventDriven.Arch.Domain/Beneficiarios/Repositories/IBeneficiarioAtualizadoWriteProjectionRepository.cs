using EventDriven.Arch.Domain.Beneficiarios.Projections;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace EventDriven.Arch.Domain.Beneficiarios.Repositories;

public interface IBeneficiarioAtualizadoWriteProjectionRepository : IWriteProjectionRepository<BeneficiarioAtualizado>
{
}