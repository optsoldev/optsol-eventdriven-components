using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioReadRepository : ReadRepository<Beneficiario>, IBeneficiarioReadRepository
{
    public BeneficiarioReadRepository(MongoContext context, ITransactionService transactionService) : base(context, transactionService, nameof(Beneficiario))
    {
       
    }
}