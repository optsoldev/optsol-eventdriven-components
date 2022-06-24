namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IAggregate
{
    public Guid Id { get; }
    public IEnumerable<IDomainEvent> PendingEvents { get; }
    public IEnumerable<IFailureEvent> FailureEvents { get; }

    public void Clear();
}
