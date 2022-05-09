namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public record PersistentEvent<T>(Guid ModelId, int ModelVersion, DateTime When, string? EventType, T Data);