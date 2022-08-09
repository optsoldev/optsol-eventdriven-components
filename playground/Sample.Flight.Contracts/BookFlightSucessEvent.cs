using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public record BookFlightSucessEvent(Guid Id, long Version) : ISuccessEvent
{
    public static explicit operator BookFlightSucessEvent(SuccessEvent @event)
    {
        return new BookFlightSucessEvent(@event.Id, @event.Version);
    }
}