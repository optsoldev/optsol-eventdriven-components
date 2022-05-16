using EventDriven.Arch.Domain.Beneficiarios.Projections;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb.ReadModelRepository
{
    public class BeneficiarioAtualizadoReadProjectionRepository : ReadProjectionRepository<BeneficiarioAtualizado>, IBeneficiarioAtualizadoReadProjectionRepository
    {
        public BeneficiarioAtualizadoReadProjectionRepository(MongoContext context) : base(context)
        {
        }
    }
}
