using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Flight.Contracts;

public record FlightBooked : ISagaContract
{
    public Guid TravelId { get; set; }

    public Guid CorrelationId { get; set; }

    public Guid ModelId { get; set; }

    public string From { get; set; }

    public string To { get; set; }
}
