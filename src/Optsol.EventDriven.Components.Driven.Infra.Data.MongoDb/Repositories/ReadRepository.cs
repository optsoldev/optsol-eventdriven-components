using System.Linq.Expressions;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class ReadRepository<T> : IReadRepository<T> where T : IAggregate
{
    protected readonly IMongoCollection<PersistentEvent<IDomainEvent>> Set;

    protected readonly ITransactionService TransactionService;

    protected ReadRepository(MongoContext context, ITransactionService transactionService, string collectionName)
    {
        Set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
        TransactionService = transactionService;
    }

    public virtual IEnumerable<IDomainEvent> GetById(Guid id) => GetEvents(e =>
        e.ModelId == id && e.IsStaging == false ||
        e.TransactionId == TransactionService.GetTransactionId() && e.ModelId == id);

    protected virtual IEnumerable<IDomainEvent> GetEvents(Expression<Func<PersistentEvent<IDomainEvent>, bool>> expression)
    {
        var sortDef = Builders<PersistentEvent<IDomainEvent>>.Sort.Descending(d => d.ModelVersion);

        return Set.Find(expression).Sort(sortDef).Project(p => p.Data).ToList();
    }
}