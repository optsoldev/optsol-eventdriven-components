namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

public interface IWriteRepository<T>
{
    void Insert(T aggregate);
    void InsertRange(List<T> aggregates);
    int SaveChanges();
}