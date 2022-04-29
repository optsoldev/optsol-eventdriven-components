namespace Optsol.EventDriven.Components.Core.Domain;

public interface IEvent
{
    public Guid ModelId { get; }
    public int ModelVersion { get; }
    public DateTime When { get; }
}