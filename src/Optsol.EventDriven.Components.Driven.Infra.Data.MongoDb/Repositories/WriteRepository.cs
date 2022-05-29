using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteRepository<T> : IWriteRepository<T> where T : IAggregate
{
    private readonly MongoContext _context;
    private readonly ITransactionService _transactionService;
    private readonly IMessageBus _messageBus;
    private readonly IMongoCollection<PersistentEvent<IDomainEvent>> _set;
    private readonly IMongoCollection<StagingEvent<IDomainEvent>> _setStaging;
    protected WriteRepository(MongoContext context, ITransactionService transactionService, IMessageBus messageBus, string collectionName)
    {
        _context = context;
        _transactionService = transactionService;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
        _setStaging = context.GetCollection<StagingEvent<IDomainEvent>>($"{collectionName}-Staging");
    }
    
    public virtual void RollbackIntegration()
    {
        _context.AddTransaction(() => _setStaging.DeleteManyAsync(f => 
            f.IntegrationId == _transactionService.GetTransactionId()));
        _context.SaveChanges();
    }

    public virtual void CommitIntegration()
    {
        var sortDef = Builders<StagingEvent<IDomainEvent>>.Sort.Descending(d => d.ModelVersion);

        var events = _setStaging
            .Find(e => e.IntegrationId == _transactionService.GetTransactionId())
            .Sort(sortDef)
            .Project(p => (PersistentEvent<IDomainEvent>)p)
            .ToList();

        _context.AddTransaction(() => _set.InsertManyAsync(events));
        _context.AddTransaction(() => _setStaging.DeleteManyAsync(f => 
            f.IntegrationId == _transactionService.GetTransactionId()));
        _context.SaveChanges();
    }

    
    public virtual void Commit(T model)
    {
        var events = model.PendingEvents.Select(e => new StagingEvent<IDomainEvent>(
            _transactionService.GetTransactionId(),
            model.Id,
            e.ModelVersion,
            e.When,
            e.GetType().AssemblyQualifiedName,
            e));
        
        _context.AddTransaction(() => _setStaging.InsertManyAsync(events));
        _context.SaveChanges();
        _messageBus.Publish(model.PendingEvents);
    }

    public virtual void Rollback(T model)
    {
        _messageBus.Publish(model.FailureEvents); 
    }
}