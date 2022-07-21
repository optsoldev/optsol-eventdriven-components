using MongoDB.Driver;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public interface IOrderBy<T>
{
    Func<SortDefinitionBuilder<T>, SortDefinition<T>> OrderBy();
}
