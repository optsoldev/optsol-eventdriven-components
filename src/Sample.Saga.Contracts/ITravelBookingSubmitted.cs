namespace Sample.Saga.Contracts;

public interface ITravelBookingSubmitted : ISaga
{
    Guid TravelId { get; }
    Guid UserId { get; }
    public string From { get; }
    public string To { get; }
    public DateTime Departure { get; }
    Guid HotelId { get; }
}
