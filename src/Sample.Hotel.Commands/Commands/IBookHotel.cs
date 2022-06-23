namespace Sample.Hotel.Contracts.Commands;

public interface IBookHotel
{
    public int HotelId { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid TravelId { get; set; }
}
