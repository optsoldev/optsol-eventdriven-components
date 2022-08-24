namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public record SuccessEvent : ISuccessEvent
{
    public Guid Id { get; }
    public long Version { get; }
    public Guid? UserId { get; }

    public SuccessEvent(Guid id, long version)
    {
        Id = id;
        Version = version;
    }

    public SuccessEvent(Guid id, long version, Guid? userId)
        : this(id, version)
    {
        UserId = userId;
    }
}