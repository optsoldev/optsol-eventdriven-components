using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
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

    public virtual IEnumerable<IDomainEvent> GetById(Guid id) {
        return Set.Find(e => e.ModelId == id && e.ModelId == id)
            .SortBy(e => e.ModelVersion).Project(e => e.Data).ToList().AsEnumerable();
}
}