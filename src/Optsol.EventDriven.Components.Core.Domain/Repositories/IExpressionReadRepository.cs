using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IExpressionReadRepository<T>
    where T : IProjection
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression);
}
