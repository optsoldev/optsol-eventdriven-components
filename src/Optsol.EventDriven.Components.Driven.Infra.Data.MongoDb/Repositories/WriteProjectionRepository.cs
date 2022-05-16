using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteProjectionRepository<T> : IWriteProjectionRepository<T> where T : IProjection, new()
{
    protected readonly IMongoCollection<T> Set;
    protected readonly MongoContext Context;

    protected WriteProjectionRepository(MongoContext context)
    {
        Context = context;
        Set = context.GetCollection<T>(nameof(T));
        
    }
    public abstract void ReceiveEvent(IEvent @event);
}