using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteReadModelRepository
{
    public void ReceiveEvent(IEvent @event);
}