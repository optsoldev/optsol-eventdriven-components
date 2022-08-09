using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteProjectionRepository<T> : IWriteProjectionRepository<T> where T : IProjection
{
    protected readonly IMongoCollection<T> Set;
    protected readonly MongoContext Context;

    protected WriteProjectionRepository(MongoContext context, string collectionName)
    {
        Context = context;
        Set = context.GetCollection<T>(collectionName);
        
    }
    public abstract void ReceiveEvent(IDomainEvent @event);
}