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
    private readonly ITransactionService _transactionService;

    public BeneficiarioWriteRepository(EventStoreDbContext eventStoreContext, ITransactionService transactionService, IMessageBus messageBus)
    {
        _eventStoreContext = eventStoreContext;
        _messageBus = messageBus;
        _transactionService = transactionService;
    }

    public void Rollback(Beneficiario model)
    {
        _eventStoreContext.Beneficiarios?.RemoveRange();
    }
    

    public void RollbackIntegration()
    {
        var events = _eventStoreContext.Beneficiarios?
            .Where(b => b.TransactionId == _transactionService.GetTransactionId());
        
        _eventStoreContext.Beneficiarios?.RemoveRange(events);
        _eventStoreContext.SaveChanges();
    }

    public void CommitIntegration()
    {
        var events = _eventStoreContext.Beneficiarios?
            .Where(b => b.TransactionId == _transactionService.GetTransactionId())
            .Select(s => (PersistentEvent<string?>)s);
        
        _eventStoreContext.Beneficiarios?.UpdateRange(events);
        _eventStoreContext.SaveChanges();
    }

    public void Commit(Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => new PersistentEvent<string>(_transactionService.GetTransactionId(),
            model.ModelId,
            e.ModelVersion,
            e.When,
            IsStaging : true,
            e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException(),
            JsonConvert.SerializeObject(e)));
        
        _eventStoreContext.Beneficiarios?.AddRange(events);
        _eventStoreContext.SaveChanges();
        _messageBus.Publish(model.PendingEvents);
        model.Commit();

    }
}