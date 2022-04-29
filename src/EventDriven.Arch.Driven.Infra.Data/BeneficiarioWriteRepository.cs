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
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }
    
    public void Commit(Guid integrationId, Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => PersistentEvent.Create(model.Id,
            ((DomainEvent)e).ModelVersion,
            ((DomainEvent)e).When,
            e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException(),
            JsonConvert.SerializeObject(e)));
        
        _eventStoreContext.Beneficiarios?.AddRange(events);
        _eventStoreContext.SaveChanges();
        _messageBus.Publish(integrationId, model.PendingEvents);
        model.Commit();

    }
}