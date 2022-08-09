using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public record BookFlightFailedEvent(Guid Id, IDictionary<string, string>? Messages) : IFailedEvent
{
    public static explicit operator BookFlightFailedEvent(FailedEvent @event)
    {
        return new BookFlightFailedEvent(@event.Id, @event.Messages);
    }
}

