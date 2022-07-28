using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using Sample.Flight.Core.Domain.Projections;

namespace Sample.Flight.Driven.Infra.Data
{
    public class FlightBookListReadRepository : ReadProjectionRepository<FlightBookList>, IFlightBookListReadRepository
    {
        public FlightBookListReadRepository(MongoContext context) : base(context, "FlightBookList")
        {
        }

    }
}
