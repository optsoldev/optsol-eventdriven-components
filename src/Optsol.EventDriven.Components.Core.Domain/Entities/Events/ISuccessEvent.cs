namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public interface ISuccessEvent : IIntegrationEvent
{
    public Guid Id { get; }
    public Guid? UserId { get; }
    public long Version { get; }
}