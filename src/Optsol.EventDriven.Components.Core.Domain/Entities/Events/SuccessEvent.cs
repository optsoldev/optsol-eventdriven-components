namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public class SuccessEvent : ISuccessEvent
{
    public Guid Id { get; set; }
    public long Version { get; set; }
    public Guid? UserId { get; set; }

    public SuccessEvent() {}

    public SuccessEvent(Guid id, long version, Guid? userId = null)
    {
        Id = id;
        Version = version;
        UserId = userId;
    }
}