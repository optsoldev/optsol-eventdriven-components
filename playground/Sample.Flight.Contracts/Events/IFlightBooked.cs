using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Flight.Contracts.Events;

public interface IFlightBooked : ISagaContract
{
    public Guid TravelId { get; }
}
