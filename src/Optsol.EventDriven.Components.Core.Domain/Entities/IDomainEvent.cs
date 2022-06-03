namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IDomainEvent
{
    public Guid Id { get; }
    public long ModelVersion { get; }
    public DateTime When { get; }
}