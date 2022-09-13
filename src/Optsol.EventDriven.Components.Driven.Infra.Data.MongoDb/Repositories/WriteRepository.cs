using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteRepository<T> : IWriteRepository<T> where T : new()
{
    protected readonly IMongoCollection<T> Set;
    protected Context Context { get; }
    
    protected WriteRepository(MongoContext context, string collectionName)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Set = context.GetCollection<T>(collectionName);
    }

    public virtual void Insert(T aggregate) => Context.AddTransaction(() => Set.InsertOneAsync(aggregate));

    public virtual void InsertRange(List<T> aggregates) => Context.AddTransaction(() => Set.InsertManyAsync(aggregates));

    public virtual int SaveChanges() => Context.SaveChanges();
}