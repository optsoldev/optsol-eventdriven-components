using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Repositories;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using System.Linq.Expressions;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public abstract class ReadRepository<T> : IReadRepository<T> where T : new()
{
    protected readonly IMongoCollection<T> Set;

    protected ReadRepository(MongoContext context, string collectionName)
    {
        Set = context.GetCollection<T>(collectionName);
    }

    public virtual IEnumerable<T> GetAll()
    {
        return Set.AsQueryable().ToEnumerable();
    }

    public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression)
    {
        return Set.Find(filterExpression).ToEnumerable();
    }

    public virtual SearchResult<T> GetAll<TSearch>(SearchRequest<TSearch> searchRequest) where TSearch : class
    {
        var search = searchRequest?.Search as ISearch<T>;
        var orderBy = searchRequest?.Search as IOrderBy<T>;

        var countFacet = AggregateFacet.Create("countFacet",
            PipelineDefinition<T, AggregateCountResult>.Create(new[]
            {
                    PipelineStageDefinitionBuilder.Count<T>()
            }));

        var stages = new List<PipelineStageDefinition<T,T>>
            {        
                PipelineStageDefinitionBuilder.Skip<T>((searchRequest.Page - 1) * (searchRequest.PageSize ?? 0)),
                PipelineStageDefinitionBuilder.Limit<T>(searchRequest.PageSize.Value)
            };

        if(orderBy is not null)
        {
            stages.Add(PipelineStageDefinitionBuilder.Sort(orderBy.OrderBy().Invoke(Builders<T>.Sort)));
        }

        var dataFacet = AggregateFacet.Create("dataFacet",
            PipelineDefinition<T, T>.Create(stages));
        
        var filterDef = GetFilterDef(search);

        var aggregation = Set.Aggregate()
            .Match(filterDef)
            .Facet(countFacet, dataFacet)
            .ToList();

        var count = aggregation.First()
            .Facets.First(x => x.Name.Equals("countFacet"))
            .Output<AggregateCountResult>()
            ?.FirstOrDefault()
            ?.Count ?? 0;

        var data = aggregation.First()
            .Facets.First(x => x.Name.Equals("dataFacet"))
            .Output<T>();

        return new SearchResult<T>()
            .SetPage(searchRequest.Page)
            .SetPageSize(searchRequest.PageSize)
            .SetPaginatedItems(data)
            .SetTotalCount((int)count);
    }

    private static FilterDefinition<T> GetFilterDef(ISearch<T>? search)
    {
        var defaultDef = Builders<T>.Filter.Empty;

        if (search is null)
        {
            return defaultDef;
        }

        var searchDef = search.Searcher().Invoke(Builders<T>.Filter);

        return searchDef;
    }
}
