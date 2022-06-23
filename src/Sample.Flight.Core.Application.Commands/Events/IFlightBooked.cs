namespace Sample.Flight.Contracts.Events;

public class IFlightBooked
{
    public Guid CorrelationId { get; }
    public Guid TravelId { get; }
}
