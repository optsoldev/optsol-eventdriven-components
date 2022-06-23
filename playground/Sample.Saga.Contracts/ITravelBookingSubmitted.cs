using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Saga.Contracts;

public interface ITravelBookingSubmitted : ISagaContract
{
    Guid TravelId { get; }
    Guid UserId { get; }
    public string From { get; }
    public string To { get; }
    public DateTime Departure { get; }
    Guid HotelId { get; }
}
