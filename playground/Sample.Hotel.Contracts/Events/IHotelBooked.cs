namespace Sample.Hotel.Contracts.Events;

public interface IHotelBooked
{
    Guid CorrelationId { get; }
    Guid TravelId { get; }
}
