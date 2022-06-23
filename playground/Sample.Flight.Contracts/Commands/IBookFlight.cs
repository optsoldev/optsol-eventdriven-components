using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Flight.Contracts.Commands;

public interface IBookFlight : ISagaContract
{
    Guid TravelId { get; }
    string From { get; }
    string To { get; }

}
