using Optsol.EventDriven.Components.Core.Application;

namespace Sample.Hotel.Contracts.Commands;

public record BookHotel : ICommand
{
    public Guid CorrelationId { get; set; }
    public int HotelId { get; set; }
    public Guid TravelId { get; set; }
}
