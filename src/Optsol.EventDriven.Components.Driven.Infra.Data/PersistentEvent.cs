namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public record PersistentEvent<T>(Guid CorrelationId, Guid Id, Guid ModelId, long ModelVersion, DateTime When, string? EventType, T Data);