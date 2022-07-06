using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Saga.Contracts.Events;

public interface ITravelBookingSubmitted : ISagaContract
{
    Guid TravelId { get; }
    Guid UserId { get; }
    public string From { get; }
    public string To { get; }
    public DateTime Departure { get; }
    int HotelId { get; }
}