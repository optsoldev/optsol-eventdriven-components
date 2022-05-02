namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

public class MongoDBException : Exception
{
    public MongoDBException(string message) : base(message)
    {
    }
}