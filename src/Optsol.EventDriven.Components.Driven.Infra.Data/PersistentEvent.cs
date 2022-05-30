namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public record PersistentEvent<T>(Guid TransactionId, Guid ModelId, int ModelVersion, DateTime When, bool IsStaging, string? EventType, T Data);