using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public record BookFlightSucessEvent(Guid Id, long Version) : SuccessEvent(Id, Version);