namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IFailureEvent
{
    public Guid IntegrationId { get; }
}