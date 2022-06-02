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
    protected WriteRepository(MongoContext context, 
        ITransactionService transactionService,
        IMessageBus messageBus, string collectionName)
    {
        _context = context;
        _transactionService = transactionService;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
    }
    
    public virtual void RollbackIntegration()
    {
        _context.AddTransaction(() => _set.DeleteManyAsync(f => 
            f.TransactionId == _transactionService.GetTransactionId()));
        _context.SaveChanges();
    }

    public virtual void CommitIntegration()
    {
        var sortDef = Builders<PersistentEvent<IDomainEvent>>.Sort.Descending(d => d.Version);

        UpdateDefinition<PersistentEvent<IDomainEvent>> updateDefinition = Builders<PersistentEvent<IDomainEvent>>.Update.Set(x => x.IsStaging, false);

        var events = _set
            .Find(e => e.TransactionId == _transactionService.GetTransactionId())
            .Sort(sortDef)
            .ToList();

        _context.AddTransaction(() => _set.UpdateManyAsync(u => u.TransactionId == _transactionService.GetTransactionId(), updateDefinition));
        _context.SaveChanges();

        _messageBus.Publish(events.Select(e => e.Data), $"response");
    }

    
    public virtual void Commit(T model)
    {
        bool isStaging = true;
        if(_transactionService.IsAutoCommit())
        {
            isStaging = false;
        }

        var events = model.PendingEvents.Select(e => new PersistentEvent<IDomainEvent>(
            _transactionService.GetTransactionId(),
            Guid.NewGuid(),
            model.Id,
            e.Version,
            e.When,
            IsStaging: isStaging,
            e.GetType().AssemblyQualifiedName,
            e));
        
        _context.AddTransaction(() => _set.InsertManyAsync(events));
        _context.SaveChanges();

        _messageBus.Publish(model.PendingEvents, $"{_transactionService.GetTransactionId()}.success");

        if(_transactionService.IsAutoCommit()) _messageBus.Publish(events.Select(e => e.Data), $"response");
    }

    public virtual void Rollback(T model)
    {
        _messageBus.Publish(model.FailureEvents, $"{_transactionService.GetTransactionId()}.failure"); 
    }
}