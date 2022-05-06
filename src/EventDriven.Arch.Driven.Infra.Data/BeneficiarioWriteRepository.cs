using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain;

namespace EventDriven.Arch.Driven.Infra.Data;

public class BeneficiarioWriteRepository : IBeneficiarioWriteRepository
{
    private readonly EventStoreDbContext _eventStoreContext;
    private readonly IMessageBus _messageBus;

    public BeneficiarioWriteRepository(EventStoreDbContext eventStoreContext, IMessageBus messageBus)
    {
        _eventStoreContext = eventStoreContext;
        _messageBus = messageBus;
    }

    public void Rollback(Guid integrationId, Beneficiario model)
    {
        _eventStoreContext.Beneficiarios?.RemoveRange();
    }
    
    public void Undo(Guid integrationId, Beneficiario model)
    {
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }

    public void RollbackIntegration(Guid integrationId)
    {
        var events = _eventStoreContext.Beneficiarios?.Where(b => b.IntegrationId == integrationId);
        _eventStoreContext.Beneficiarios?.RemoveRange(events);
        _eventStoreContext.SaveChanges();
    }

    public void CommitIntegration(Guid integrationId)
    {
        var events = _eventStoreContext.Beneficiarios.Where(b => b.IntegrationId == integrationId);

        var updatedEvents = events.Select(e => PersistentEvent.Create(integrationId,
            e.ModelId,
            e.ModelVersion,
            e.When,
            true,
            e.GetType().AssemblyQualifiedName,
            JsonConvert.SerializeObject(e)));
        
        _eventStoreContext.Beneficiarios.UpdateRange(updatedEvents);
        _eventStoreContext.SaveChanges();
    }

    public void Commit(Guid integrationId, Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => PersistentEvent.Create(integrationId,
            model.Id,
            ((DomainEvent)e).ModelVersion,
            ((DomainEvent)e).When,
            false,
            e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException(),
            JsonConvert.SerializeObject(e)));
        
        _eventStoreContext.Beneficiarios?.AddRange(events);
        _eventStoreContext.SaveChanges();
        _messageBus.Publish(integrationId, model.PendingEvents);
        model.Commit();

    }
}