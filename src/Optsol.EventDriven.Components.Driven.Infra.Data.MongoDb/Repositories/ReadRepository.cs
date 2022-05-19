using System.Linq.Expressions;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class ReadRepository<T> : IReadRepository<T> where T : IAggregate
{
    protected readonly IMongoCollection<PersistentEvent<IDomainEvent>> Set;

    protected ReadRepository(MongoContext context, string collectionName)
    {
        Set = context.GetCollection<PersistentEvent<IDomainEvent>>(collectionName);
    }
    
    public IEnumerable<IDomainEvent> GetById(Guid id) => GetEvents(e => e.ModelId == id);
    
    protected IEnumerable<IDomainEvent> GetEvents(Expression<Func<PersistentEvent<IDomainEvent>, bool>> expression)
    {
        var sortDef = Builders<PersistentEvent<IDomainEvent>>.Sort.Descending(d => d.ModelVersion);

        return Set.Find(expression).Sort(sortDef).Project(p => p.Data).ToList();
    }
}