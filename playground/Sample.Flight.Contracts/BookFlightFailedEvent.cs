using FluentValidation.Results;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public class BookFlightFailedEvent : FailedEvent
{
    public BookFlightFailedEvent(Guid Id, IEnumerable<ValidationFailure> ValidationFailures, Guid UserId) : base(Id,
        ValidationFailures, UserId)
    {}
}
