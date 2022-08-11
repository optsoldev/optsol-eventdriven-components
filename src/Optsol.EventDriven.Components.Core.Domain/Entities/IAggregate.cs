using FluentValidation.Results;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IAggregate
{
    public Guid Id { get; }
    public long Version { get; }
    public bool Invalid { get; }
    public ValidationResult ValidationResult { get; }
    IEnumerable<IDomainEvent> PendingEvents { get; }
    public void Clear();
}
