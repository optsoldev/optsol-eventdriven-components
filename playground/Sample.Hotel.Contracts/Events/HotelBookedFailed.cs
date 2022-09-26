namespace Sample.Hotel.Contracts.Events;

public record HotelBookedFailed
{
    public Guid CorrelationId { get; set; }
}
