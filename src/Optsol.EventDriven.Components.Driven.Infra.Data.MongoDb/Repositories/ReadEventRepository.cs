using System.Linq.Expressions;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class ReadEventRepository<T> : IReadRepository<T> where T : IAggregate
{
    private readonly IMongoCollection<PersistentEvent<IEvent>> _set;

    protected ReadEventRepository(MongoContext context, string collectionName)
    {
        _set = context.GetCollection<PersistentEvent<IEvent>>(collectionName);
    }
    
    public IEnumerable<IEvent> GetById(Guid id) => GetEvents(e => e.ModelId == id);
    
    protected IEnumerable<IEvent> GetEvents(Expression<Func<PersistentEvent<IEvent>, bool>> expression)
    {
        var sortDef = Builders<PersistentEvent<IEvent>>.Sort.Descending(d => d.ModelVersion);

        return _set.Find(expression).Sort(sortDef).Project(p => p.Data).ToList();
    }
}