namespace EventDriven.Arch.Domain;

public interface IEvent
{
    public Guid ModelId { get; }
    public int ModelVersion { get; }
    public DateTime When { get; }
}