namespace Sample.Saga.Contracts.Events;

public interface ITravelBookingSubmitted
{
    Guid CorrelationId { get; }
    Guid TravelId { get; }
    Guid UserId { get; }
    public string From { get; }
    public string To { get; }
    public DateTime Departure { get; }
    int HotelId { get; }
}

public record TravelBookStatusRequested
{
    public Guid TravelId { get; set; }
}

public record TravelBookStatus
{
    public Guid TravelId {get;set;}
    public string CurrentState { get; set; }
}

public record TravelBookNotFound
{
    public Guid TravelId { get; set; }
    public string CurrentState { get; set; }
}