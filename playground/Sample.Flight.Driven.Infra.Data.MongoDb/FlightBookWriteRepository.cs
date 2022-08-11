using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using Sample.Flight.Core.Domain;

namespace Sample.Flight.Driven.Infra.Data;

public class FlightBookWriteRepository : WriteEventRepository<FlightBook>, IFlightBookWriteRepository
{
    public FlightBookWriteRepository(MongoContext context,  INotificator notificator,  FlightBookProjectionRepositoryCollection? collection) : base(context, nameof(FlightBook), notificator, collection)
    {
    }
}
