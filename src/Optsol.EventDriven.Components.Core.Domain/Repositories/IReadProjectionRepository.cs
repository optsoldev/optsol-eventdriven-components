using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadProjectionRepository<T> where T : IProjection
{ 
    T GetById(Guid id);

    IEnumerable<T> GetAll();

    IEnumerable<T> GetAllByIds(params Guid[] ids);
}
