using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Domain;

namespace EventDriven.Arch.Driven.Infra.Data;

public class PersistentEvent : IEvent
{
    public Guid ModelId { get; private set; }
    public int ModelVersion { get; private set; }
    public DateTime When { get; private set; }
    public string EventType { get; private set; }
    public string Data { get; private set; }

    public static PersistentEvent Create(Guid id, int version, DateTime when, string eventType, string data) =>
        new PersistentEvent
        {
            ModelId = id,
            ModelVersion = version,
            When = when,
            EventType = eventType,
            Data = data
        };
}