using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class WriteReadModelRepository : IWriteReadModelRepository
{
    public abstract void ReceiveEvent(IEvent @event);
}