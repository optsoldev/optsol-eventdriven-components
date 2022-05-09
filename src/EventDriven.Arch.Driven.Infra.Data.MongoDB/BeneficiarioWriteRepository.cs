using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioWriteRepository : WriteRepository<Beneficiario>, IBeneficiarioWriteRepository
{
    public BeneficiarioWriteRepository(MongoContext context, IMessageBus messageBus) : base(context, messageBus, nameof(Beneficiario))
    {
    }
}

