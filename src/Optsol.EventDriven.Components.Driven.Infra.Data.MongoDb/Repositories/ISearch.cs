using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public interface ISearch<T>
where T : IProjection
{
    Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> Searcher();
}
