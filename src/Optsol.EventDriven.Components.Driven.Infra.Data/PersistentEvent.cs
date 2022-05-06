using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public class PersistentEvent<T>
{
    public Guid IntegrationId { get; private init; }
    public Guid ModelId { get; private init; }
    public int ModelVersion { get; private init; }
    public DateTime When { get; private init; }
    public bool Status { get; private init; }
    public string? EventType { get; private init; } = null;
    public T Data { get; private init; }

    public static PersistentEvent<T> Create(Guid integrationId, Guid id, int version, DateTime when, bool status, string eventType, T data) =>
        new PersistentEvent<T>
        {
            IntegrationId = integrationId,
            ModelId = id,
            ModelVersion = version,
            When = when,
            EventType = eventType,
            Data = data
        };
}