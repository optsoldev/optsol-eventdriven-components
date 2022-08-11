namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public interface IDomainEvent
{
    public Guid ModelId { get; }
    public long ModelVersion { get; }
    public DateTime When { get; }
}