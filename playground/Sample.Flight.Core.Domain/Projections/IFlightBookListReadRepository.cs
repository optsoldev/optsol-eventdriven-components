using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Sample.Flight.Core.Domain.Projections;

public interface IFlightBookListReadRepository : IReadProjectionRepository<FlightBookList>
{
}

public interface IFlightBookListWriteRepository : IFlightBookProjectionRepository<FlightBookList>
{
    
}

public interface IFlightBookProjectionRepository<T> : IWriteProjectionRepository<T> where T : IFlightBookProjection
{
    
}
