using Optsol.EventDriven.Components.Core.Application;

namespace Sample.Flight.Contracts.Commands;

public record UnbookFlight : ICommand
{
    public Guid CorrelationId { get; set; }
    public Guid ModelId { get; set; }
}
