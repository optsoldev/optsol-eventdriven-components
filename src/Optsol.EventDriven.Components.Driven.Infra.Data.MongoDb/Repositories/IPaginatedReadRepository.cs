using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public interface IPaginatedReadRepository<T> : IExpressionReadRepository<T>
    where T : IProjection
{
    SearchResult<T> GetAll<TSearch>(SearchRequest<TSearch> searchRequest)
        where TSearch : class;
}
