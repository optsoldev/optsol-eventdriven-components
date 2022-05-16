namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

public sealed class SearchRequest<TSearch>
       where TSearch : class
{
    public SearchRequest(TSearch search, int page, int? size)
    {
        Page = page;
        PageSize = size;
        Search = search;
    }

    public int Page { get; set; }

    public int? PageSize { get; set; }

    public TSearch Search { get; set; }
}
