using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.InMemory;

public class BeneficiarioWriteRepository : IBeneficiarioWriteRepository
{
    private readonly EventStoreDbContext _eventStoreContext;
    private readonly IMessageBus _messageBus;

    public BeneficiarioWriteRepository(EventStoreDbContext eventStoreContext, IMessageBus messageBus)
    {
        _eventStoreContext = eventStoreContext;
        _messageBus = messageBus;
    }

    public void Rollback(Beneficiario model)
    {
        _eventStoreContext.Beneficiarios?.RemoveRange();
    }
    

    public void RollbackIntegration(Guid integrationId)
    {
        var events = _eventStoreContext.BeneficiariosStaging?
            .Where(b => b.IntegrationId == integrationId);
        
        _eventStoreContext.BeneficiariosStaging?.RemoveRange(events);
        _eventStoreContext.SaveChanges();
    }

    public void CommitIntegration(Guid integrationId)
    {
        var events = _eventStoreContext.BeneficiariosStaging?
            .Where(b => b.IntegrationId == integrationId)
            .Select(s => (PersistentEvent<string?>)s);
        
        _eventStoreContext.Beneficiarios?.UpdateRange(events);
        _eventStoreContext.SaveChanges();
    }

    public void Commit(Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => new StagingEvent<string>(e.IntegrationId,
            model.Id,
            e.ModelVersion,
            e.When,
            e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException(),
            JsonConvert.SerializeObject(e)));
        
        _eventStoreContext.BeneficiariosStaging?.AddRange(events);
        _eventStoreContext.SaveChanges();
        _messageBus.Publish(model.PendingEvents);
        model.Commit();

    }
}