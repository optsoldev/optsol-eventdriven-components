using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public record PersistentEvent(Guid ModelId, int ModelVersion, DateTime When, string? EventType, IEvent Data) : PersistentEvent<IEvent>(ModelId, ModelVersion, When, EventType, Data);