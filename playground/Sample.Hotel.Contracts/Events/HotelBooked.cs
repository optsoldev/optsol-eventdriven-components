namespace Sample.Hotel.Contracts.Events;

public record HotelBooked
{
    public Guid TravelId { get; set; }

    public Guid CorrelationId { get; set; }
}
