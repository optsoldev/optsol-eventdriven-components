using MongoDB.Driver;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

public class MongoContext : Context
{
    public MongoContext(MongoSettings mongoSettings, IMongoClient mongoClient)
        : base(mongoSettings, mongoClient)
    {
    }
}