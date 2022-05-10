namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IEvent
{
    public Guid IntegrationId { get; }
    public Guid ModelId { get; }
    public int ModelVersion { get; }
    public DateTime When { get; }
}