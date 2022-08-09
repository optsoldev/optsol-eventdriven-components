using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Core.Domain;

public record FlightBookCreated(Guid UserId, string? From, string? To)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);

public record FlightUnbooked(Guid ModelId, long ModelVersion) : DomainEvent(ModelId, ModelVersion, DateTime.UtcNow);