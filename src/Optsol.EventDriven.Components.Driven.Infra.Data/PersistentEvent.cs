namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public record PersistentEvent<T>(Guid TransactionId, Guid Id, Guid ModelId, long Version, DateTime When, bool IsStaging, string? EventType, T Data);