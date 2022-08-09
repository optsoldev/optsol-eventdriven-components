using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadEventRepository<T> where T : IAggregate
{
    public IEnumerable<IDomainEvent> GetById(Guid id);
}