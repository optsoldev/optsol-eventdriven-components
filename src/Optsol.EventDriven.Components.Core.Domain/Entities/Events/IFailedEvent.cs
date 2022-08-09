namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public interface IFailedEvent : IIntegrationEvent
{
    public Guid Id { get;  }
    public IDictionary<string, string>? Messages { get;  }
}