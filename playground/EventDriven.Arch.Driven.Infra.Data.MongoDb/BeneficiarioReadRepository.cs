using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioReadRepository : ReadEventRepository<Beneficiario>, IBeneficiarioReadRepository
{
    public BeneficiarioReadRepository(MongoContext context) : base(context, nameof(Beneficiario))
    {
       
    }
}