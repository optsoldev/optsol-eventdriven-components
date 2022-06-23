using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Hotel.Contracts.Commands;

public interface IBookHotel : ISagaContract
{
    public int HotelId { get; set; }
    public Guid TravelId { get; set; }
}
