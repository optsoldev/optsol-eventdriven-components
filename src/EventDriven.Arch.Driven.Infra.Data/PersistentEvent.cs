using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data;

public record PersistentEvent(Guid ModelId, int ModelVersion, DateTime When, string? EventType, string Data) : PersistentEvent<string>(ModelId, ModelVersion, When, EventType, Data);