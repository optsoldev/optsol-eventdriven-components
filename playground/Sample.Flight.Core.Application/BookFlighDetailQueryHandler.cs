using MediatR;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using Sample.Flight.Core.Domain.Projections;

namespace Sample.Flight.Core.Application
{
    public class BookFlightListSearch : ISearch<FlightBookList>
    {
        public Func<FilterDefinitionBuilder<FlightBookList>, FilterDefinition<FlightBookList>> Searcher()
        {
            throw new NotImplementedException();
        }
    }

    public record BookFlightListQuery(SearchRequest<BookFlightListSearch> Request) : IQuery<SearchResult<FlightBookList>>;


    public class BookFlighListQueryHandler : IRequestHandler<BookFlightListQuery, SearchResult<FlightBookList>>
    {
        private readonly IFlightBookListReadRepository readRepository;

        public BookFlighListQueryHandler(IFlightBookListReadRepository readRepository)
        {
            this.readRepository = readRepository;
        }

        public Task<SearchResult<FlightBookList>> Handle(BookFlightListQuery request, CancellationToken cancellationToken)
        {
           return Task.FromResult(readRepository.GetAll(request.Request));
        }
    }
}
