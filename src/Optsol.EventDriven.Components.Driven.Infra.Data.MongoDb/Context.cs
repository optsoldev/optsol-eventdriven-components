using MongoDB.Driver;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

public abstract class Context : IDisposable
{
    private bool _disposed = false;
    private IClientSessionHandle? _session;
    private readonly TransactionList _transactions;
    public IMongoClient MongoClient { get; }

    public IMongoDatabase Database { get; }

    protected Context(MongoSettings mongoSettings, IMongoClient mongoClient)
    {
        if (mongoSettings is null)
        {
            throw new MongoDBException($"{nameof(mongoSettings)} está nulo");
        }

        _transactions = TransactionList.Create();

        MongoClient = mongoClient ?? throw new MongoDBException($"{nameof(mongoClient)} está nulo");
        Database = mongoClient.GetDatabase(mongoSettings.DatabaseName);
    }
    
    public int SaveChanges()
    {
        var countSaveTasks = 0;
        using (_session = MongoClient.StartSession())
        {
            _session.StartTransaction();

            _transactions.Execute();
            countSaveTasks = _transactions.Transactions.Count;
            _transactions.Clear();

            _session.CommitTransaction();
        }

        return countSaveTasks;
    }

    public IMongoCollection<TAggregate> GetCollection<TAggregate>(string databaseName) => 
        Database.GetCollection<TAggregate>(databaseName);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed is not true && disposing)
        {
            _session?.Dispose();
        }
        _disposed = true;
    }

    public void AddTransaction(Func<Task> transaction) => _transactions.AddTransaction(transaction);
}