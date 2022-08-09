namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public interface ISuccessEvent : IIntegrationEvent
{
    public Guid Id { get; }
    public long Version { get; }
}