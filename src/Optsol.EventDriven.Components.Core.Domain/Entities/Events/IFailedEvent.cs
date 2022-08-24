namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public interface IFailedEvent : IIntegrationEvent
{
    public Guid Id { get; }
    public Guid? UserId { get; }
    public IEnumerable<string> Messages { get;  }
}