using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IAggregate
{
    public Guid Id { get; }
    public bool Invalid { get; }
    public IEnumerable<IDomainEvent> PendingEvents { get; }
    public IEnumerable<IFailedEvent> FailedEvents { get; }
    public IEnumerable<ISuccessEvent> SuccessEvents { get; }
    public void Clear();
    public void RaiseSuccessEvent();
    public void RaiseFailedEvent();
}
