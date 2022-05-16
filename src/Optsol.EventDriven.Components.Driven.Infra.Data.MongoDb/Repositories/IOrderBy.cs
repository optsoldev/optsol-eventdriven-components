using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public interface IOrderBy<T>
    where T : IProjection
{
    Func<SortDefinitionBuilder<T>, SortDefinition<T>> OrderBy();
}
