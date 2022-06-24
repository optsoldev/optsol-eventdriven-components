using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Hotel.Contracts.Events;

public record HotelBooked : ISagaContract
{
    public Guid TravelId { get; set; }

    public Guid CorrelationId { get; set; }
}
