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
    private readonly IMongoCollection<StagingEvent<IEvent>> _setStaging;

    public BeneficiarioWriteRepository(MongoContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent<IEvent>>(nameof(Beneficiario));
        _setStaging = context.GetCollection<StagingEvent<IEvent>>($"{nameof(Beneficiario)}-Staging");
    }

    public void RollbackIntegration(Guid integrationId)
    {
        _context.AddTransaction(() => _setStaging.DeleteManyAsync(f => f.IntegrationId == integrationId));
        _context.SaveChanges();
    }

    public void CommitIntegration(Guid integrationId)
    {
        var sortDef = Builders<StagingEvent<IEvent>>.Sort.Descending(d => d.ModelVersion);

        var events = _setStaging
            .Find(e => e.IntegrationId == integrationId)
            .Sort(sortDef)
            .ToList();

        _context.AddTransaction(() => _setStaging.InsertManyAsync(events));
        _context.SaveChanges();
    }
    
    public void Commit(Guid integrationId, Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => new StagingEvent<IEvent>(
            integrationId,
            model.Id,
            ((DomainEvent)e).ModelVersion,
            ((DomainEvent)e).When,
            e.GetType().AssemblyQualifiedName,
            e));
        
        _context.AddTransaction(() => _setStaging.InsertManyAsync(events));
        _context.SaveChanges();
        _messageBus.Publish(integrationId, model.PendingEvents);
    }

    public void Rollback(Guid integrationId, Beneficiario model)
    {
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }
}

