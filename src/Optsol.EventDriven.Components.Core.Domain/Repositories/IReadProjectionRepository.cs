using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadProjectionRepository<T> : IExpressionReadRepository<T> where T : IProjection
{ 
    T GetById(Guid id);

    IEnumerable<T> GetAll();

    IEnumerable<T> GetAllByIds(params Guid[] ids);

    SearchResult<T> GetAll<TSearch>(SearchRequest<TSearch> searchRequest) where TSearch : class;

}
