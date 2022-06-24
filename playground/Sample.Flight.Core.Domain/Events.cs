using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Sample.Flight.Core.Domain;

public record FlightBookCreated(Guid UserId, string? From, string? To)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);