using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Driven.Infra.Data;

public class FlightBookWriteRepository : WriteRepository<FlightBook>, IFlightBookWriteRepository
{
    public FlightBookWriteRepository(MongoContext context) : base(context, nameof(FlightBook))
    {
    }
}
