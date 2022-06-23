using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public class SearchResult<T>
        where T : IProjection
{
    public int Page { get; private set; }

    public int? PageSize { get; private set; }

    public long TotalCount { get; private set; }

    public long PageCount => Data.Count();

    public IEnumerable<T> Data { get; private set; }

    public SearchResult<T> SetPage(int page)
    {
        Page = page;

        return this;
    }

    public SearchResult<T> SetPageSize(int? pageSize)
    {
        PageSize = pageSize;

        return this;
    }

    public SearchResult<T> SetTotalCount(int totalCount)
    {
        TotalCount = totalCount;

        return this;
    }

    public SearchResult<T> SetPaginatedItems(IEnumerable<T> data)
    {
        Data = data;

        return this;
    }
}
