namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public class SuccessEvent : ISuccessEvent
{
    public Guid Id { get; set; }
    public long Version { get; set; }
    public Guid? UserId { get; set; }

    public SuccessEvent() {}
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