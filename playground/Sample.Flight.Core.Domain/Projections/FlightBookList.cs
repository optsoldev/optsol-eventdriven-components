using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace Sample.Flight.Core.Domain.Projections;

public class FlightBookList : IProjection
{
    public string From { get; set; }
    public string To { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class FlightBookListSearch : ISearch<FlightBookList>, IOrderBy<FlightBookList>
{
    public Func<SortDefinitionBuilder<FlightBookList>, SortDefinition<FlightBookList>> OrderBy()
    {
        throw new NotImplementedException();
    }

    public Func<FilterDefinitionBuilder<FlightBookList>, FilterDefinition<FlightBookList>> Searcher()
    {
        throw new NotImplementedException();
    }
}
