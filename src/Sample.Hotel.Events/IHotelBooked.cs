namespace Sample.Hotel.Events;

public interface IHotelBooked
{
    Guid CorrelationId { get; }
    Guid TravelId { get; }
}
