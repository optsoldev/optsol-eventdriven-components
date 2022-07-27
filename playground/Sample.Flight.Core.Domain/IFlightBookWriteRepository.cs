using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Sample.Flight.Core.Domain.Projections;

namespace Sample.Flight.Core.Domain;

public interface IFlightBookWriteRepository : IWriteEventRepository<FlightBook>
{
}
