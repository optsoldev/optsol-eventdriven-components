using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using System.Linq.Expressions;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IReadProjectionRepository<T> where T : IProjection
{ 
    T GetById(Guid id);

    IEnumerable<T> GetAll();

    IEnumerable<T> GetAllByIds(params Guid[] ids);

    IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression);

    SearchResult<T> GetAll<TSearch>(SearchRequest<TSearch> searchRequest) where TSearch : class;


}
