﻿using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;
using Sample.Flight.Core.Domain.Projections;

namespace Sample.Flight.Driven.Infra.Data
{
    public class FlightBookListReadRepository : ReadProjectionRepository<FlightBookList>, IFlightBookListReadRepository
    {
        public FlightBookListReadRepository(MongoContext context) : base(context, "FlightBookList")
        {
        }

    }

    public class FlightBookListWriteRepository : WriteProjectionRepository<FlightBookList>, IFlightBookListWriteRepository
    {
        public FlightBookListWriteRepository(MongoContext context) : base(context, "FlightBookList")
        {
        }

        public override void ReceiveEvent(IDomainEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
