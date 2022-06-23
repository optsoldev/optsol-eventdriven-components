namespace Sample.Flight.Core.Application.Commands;

public interface IBookFlight
{
    public Guid CorrelationId { get; }
    public string From { get; } 
    public string To { get; } 
    public DateTime Departure { get; } 
    public Guid TravelId { get; }
}