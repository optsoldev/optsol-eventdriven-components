namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract record DomainEvent() : IDomainEvent
{
    public DomainEvent(Guid id, long version, DateTime when) : this()
    {
        this.Id = id;
        this.Version = version;
        this.When = when;
    }

    public Guid Id { get; init; }

    public long Version { get; init; }

    public DateTime When { get; init; }
}