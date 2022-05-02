using MongoDB.Driver;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class MongoContext : Context
{
    public MongoContext(MongoSettings mongoSettings, IMongoClient mongoClient)
        : base(mongoSettings, mongoClient)
    {
    }
}