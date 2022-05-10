using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadRepository<T> where T : IAggregate
{
    public IEnumerable<IEvent> GetById(Guid id);
}