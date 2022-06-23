namespace Sample.Hotel.Commands;

public interface IBookHotel
{
    public int HotelId { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid TravelId { get; set; }
}
