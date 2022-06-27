using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.InMemory;

public class BeneficiarioWriteRepository : IBeneficiarioWriteRepository
{
    private readonly EventStoreDbContext _eventStoreContext;

    public BeneficiarioWriteRepository(EventStoreDbContext eventStoreContext)
    {
        _eventStoreContext = eventStoreContext;
    }

    public void Rollback(Beneficiario model)
    {
        _eventStoreContext.Beneficiarios?.RemoveRange();
    }
    

    public void Commit(Guid correlationId, Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => new PersistentEvent<string>(correlationId,
            Guid.NewGuid(),
            model.Id,
            e.ModelVersion,
            e.When,
            e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException(),
            JsonConvert.SerializeObject(e)));
        
        _eventStoreContext.Beneficiarios?.AddRange(events);
        _eventStoreContext.SaveChanges();

        model.Commit();

    }
}