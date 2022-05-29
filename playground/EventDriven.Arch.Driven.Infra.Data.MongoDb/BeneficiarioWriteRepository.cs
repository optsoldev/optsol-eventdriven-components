using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioWriteRepository : WriteRepository<Beneficiario>, IBeneficiarioWriteRepository
{
    public BeneficiarioWriteRepository(MongoContext context, ITransactionService transactionService, IMessageBus messageBus) : base(context, transactionService, messageBus, nameof(Beneficiario))
    {
    }
}

