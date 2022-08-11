using FluentValidation.Results;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public record BookFlightFailedEvent(Guid Id, IEnumerable<ValidationFailure> ValidationFailures) : FailedEvent(Id,
    ValidationFailures);