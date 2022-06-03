using System.Linq.Expressions;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.InMemory;

public class BeneficiarioReadRepository : IBeneficiarioReadRepository
{
    private readonly EventStoreDbContext _eventStoreContext;

    public BeneficiarioReadRepository(EventStoreDbContext eventStoreContext)
    {
        _eventStoreContext = eventStoreContext;
    }
    
    public IEnumerable<IDomainEvent> GetById(Guid id) =>
        GetEvents(e => e.Id == id);

    public IEnumerable<IDomainEvent> GetByVersion(Guid id, int version) =>
        GetEvents(e => e.Id == id && e.ModelVersion <= version);

    public IEnumerable<IDomainEvent> GetByTime(Guid id, DateTime until) =>
        GetEvents(e => e.Id == id && e.When <= until);

    public IEnumerable<IDomainEvent> GetFromVersion(Guid id, int version) =>
        GetEvents(e => e.Id == id && e.ModelVersion > version);

    private IEnumerable<IDomainEvent> GetEvents(Expression<Func<PersistentEvent<string>, bool>> expression) =>
        (_eventStoreContext.Beneficiarios ?? throw new InvalidOperationException()).Where(expression)
            .OrderBy(e => e.ModelVersion)
            .Select(e => JsonConvert.DeserializeObject(e.Data, Type.GetType(e.EventType)))
            .Cast<DomainEvent>();
}