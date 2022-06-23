using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Flight.Contracts.Commands;

public interface IBookFlight : ISagaContract
{
    public Guid TravelId { get; }
    public string From { get; }
    public string To { get; }

}
