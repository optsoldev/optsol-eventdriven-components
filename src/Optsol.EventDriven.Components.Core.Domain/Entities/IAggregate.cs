namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IAggregate
{
    public Guid Id { get; }
    public IEnumerable<IEvent> PendingEvents { get; }
    public IEnumerable<IFailureEvent> FailureEvents { get; }
}
