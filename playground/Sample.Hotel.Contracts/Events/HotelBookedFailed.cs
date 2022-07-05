using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Hotel.Contracts.Events;

public record HotelBookedFailed : ISagaContract
{
    public Guid CorrelationId { get; set; }
}
