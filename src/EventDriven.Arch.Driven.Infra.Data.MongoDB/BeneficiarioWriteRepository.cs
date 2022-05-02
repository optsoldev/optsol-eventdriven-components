using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MongoDB.Bson;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioWriteRepository : IBeneficiarioWriteRepository
{
    private readonly MongoContext _context;
    private readonly IMessageBus _messageBus;
    private readonly IMongoCollection<PersistentEvent<BsonDocument>> _set;
    
    public BeneficiarioWriteRepository(MongoContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent<BsonDocument>>(nameof(Beneficiario));
    }

    public void Commit(Guid integrationId, Beneficiario model)
    {
        var events = model.PendingEvents.Select(e => PersistentEvent<BsonDocument>.Create(model.Id,
            ((DomainEvent)e).ModelVersion,
            ((DomainEvent)e).When,
            e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException(),
            e.ToBsonDocument()));
        
        _context.AddTransaction(() => _set.InsertManyAsync(events));
        _context.SaveChanges();
    }

    public void Rollback(Guid integrationId, Beneficiario model)
    {
        _messageBus.Publish(integrationId, model.FailureEvents); 
    }
}

