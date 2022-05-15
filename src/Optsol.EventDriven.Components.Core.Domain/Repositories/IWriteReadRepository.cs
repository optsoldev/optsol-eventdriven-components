using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteReadModelRepository<T> where T : IReadModel, new()
{
    public void ReceiveEvent(IEvent @event);
}