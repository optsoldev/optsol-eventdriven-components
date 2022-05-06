using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioWriteRepository : IBeneficiarioWriteRepository
{
    private readonly MongoContext _context;
    private readonly IMessageBus _messageBus;
    private readonly IMongoCollection<PersistentEvent<IEvent>> _set;
    
    public BeneficiarioWriteRepository(MongoContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent<IEvent>>(nameof(Beneficiario));
    }

    public void RollbackIntegration(Guid integrationId)
    {
        _context.AddTransaction(() => _set.DeleteManyAsync(f => f.IntegrationId == integrationId));
        _context.SaveChanges();
    }

    public void CommitIntegration(Guid integrationId)
    {
        _context.AddTransaction(() =>  _set.UpdateManyAsync(
            f => f.IntegrationId == integrationId,
            Builders<PersistentEvent<IEvent>>.Update.Set(p => p.Status, true)));
        _context.SaveChanges();
    }
    
    public void Commit(Guid integrationId, Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => PersistentEvent.Create(
            integrationId,
            model.Id,
            ((DomainEvent)e).ModelVersion,
            ((DomainEvent)e).When,
            false,
            e.GetType().AssemblyQualifiedName,
            e));
        
        _context.AddTransaction(() => _set.InsertManyAsync(events));
        _context.SaveChanges();
        _messageBus.Publish(integrationId, model.PendingEvents);
    }

    public void Rollback(Guid integrationId, Beneficiario model)
    {
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }
}

