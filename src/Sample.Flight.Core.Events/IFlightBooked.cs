namespace Sample.Flight.Core.Events;

public interface IFlightBooked
{
    Guid CorrelationId { get; }
    Guid TravelId { get; }
}