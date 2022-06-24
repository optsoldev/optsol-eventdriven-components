using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Hotel.Contracts.Events;

public interface IHotelBooked : ISagaContract
{
    public Guid TravelId { get; }
}
