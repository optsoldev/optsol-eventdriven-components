namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract record DomainEvent() : IDomainEvent
{
    public DomainEvent(Guid modelId, long modelVersion, DateTime when) : this()
    {
        this.ModelId = modelId;
        this.ModelVersion = modelVersion;
        this.When = when;
    }

    public Guid ModelId { get; init; }

    public long ModelVersion { get; init; }

    public DateTime When { get; init; }
}