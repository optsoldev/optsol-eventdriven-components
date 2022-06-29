using Optsol.EventDriven.Components.Core.Application;

namespace Sample.Flight.Contracts.Commands;

public record BookFlight : ICommand
{
    public Guid CorrelationId { get; set; }
    public Guid TravelId { get; set; }
    public Guid UserId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }

}
