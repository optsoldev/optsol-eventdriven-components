namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public abstract record DomainEvent() : IDomainEvent
{
    protected DomainEvent(Guid modelId, long modelVersion, DateTime when) : this()
    {
        ModelId = modelId;
        ModelVersion = modelVersion;
        When = when;
    }

    public Guid ModelId { get; }

    public long ModelVersion { get; }

    public DateTime When { get; }
}