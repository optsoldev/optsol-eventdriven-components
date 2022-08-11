using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using Sample.Flight.Core.Domain.Projections;

namespace Sample.Flight.Driven.Infra.Data;

public class FlightBookProjectionRepositoryCollection : IProjectionWriteRepositoryCollection
{
    public IList<Action<IDomainEvent>> Actions { get; } = new List<Action<IDomainEvent>>();

    public FlightBookProjectionRepositoryCollection(IEnumerable<IFlightBookWriteProjectionRepository> repositories)
    {
        foreach (var repository in repositories)
        {
            Actions.Add(repository.ReceiveEvent);
        }
    }
}