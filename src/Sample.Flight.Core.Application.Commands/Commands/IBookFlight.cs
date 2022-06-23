using Sample.Saga.Contracts;

namespace Sample.Flight.Contracts.Commands;

public interface IBookFlight : ISagaContracts
{
    Guid TravelId { get; }
    string From { get; }
    string To { get; }

}
