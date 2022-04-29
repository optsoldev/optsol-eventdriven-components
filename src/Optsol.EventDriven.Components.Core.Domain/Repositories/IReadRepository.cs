using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadRepository<T> where T : IAggregate
{
    public IEnumerable<IEvent> GetById(Guid id);
    public IEnumerable<IEvent> GetByVersion(Guid id, int version);
    public IEnumerable<IEvent> GetByTime(Guid id, DateTime until);
    public IEnumerable<IEvent> GetFromVersion(Guid id, int version);
}