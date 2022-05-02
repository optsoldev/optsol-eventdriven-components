using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public class PersistentEvent<T> : IEvent
{
    public Guid ModelId { get; private init; }
    public int ModelVersion { get; private init; }
    public DateTime When { get; private init; }
    public string? EventType { get; private init; } = null;
    public T? Data { get; private init; } = default(T);

    public static PersistentEvent<T> Create(Guid id, int version, DateTime when, string eventType, T data) =>
        new PersistentEvent<T>
        {
            ModelId = id,
            ModelVersion = version,
            When = when,
            EventType = eventType,
            Data = data
        };
}