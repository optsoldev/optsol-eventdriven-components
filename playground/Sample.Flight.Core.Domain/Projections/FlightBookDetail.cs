namespace Sample.Flight.Core.Domain.Projections;

public class FlightBookDetail : IFlightBookProjection
{
    public Guid Id { get; }
    public DateTime CreatedDate { get; }
}
