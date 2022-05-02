namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

public class TransactionList
{
    public IList<Func<Task>> Transactions { get; private set; }

    public int Total => Transactions.Count;

    public void AddTransaction(Func<Task> transaction) => Transactions.Add(transaction);

    public void Execute() => Task.WhenAll(Transactions.Select(execute => execute()));

    public void Clear() => Transactions.Clear();

    public static TransactionList Create()
    {
        var transactionList = new TransactionList
        {
            Transactions = new List<Func<Task>>()
        };

        return transactionList;
    }
}