using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using System.Linq.Expressions;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadRepository<T>
{
    IEnumerable<T> GetAll();

    IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression);

    SearchResult<T> GetAll<TSearch>(SearchRequest<TSearch> searchRequest) where TSearch : class;
}

