namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public abstract record DomainEvent() : IDomainEvent
{
    protected DomainEvent(Guid modelId, long modelVersion, DateTime when) : this()
    {
        ModelId = modelId;
        ModelVersion = modelVersion;
        When = when;
    }

    public Guid ModelId { get; set; }

    public long ModelVersion { get; set; }

    public DateTime When { get; set; }
}