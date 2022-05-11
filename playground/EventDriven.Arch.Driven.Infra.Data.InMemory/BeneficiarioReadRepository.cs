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
    
    public IEnumerable<IEvent> GetById(Guid id) =>
        GetEvents(e => e.ModelId == id);

    public IEnumerable<IEvent> GetByVersion(Guid id, int version) =>
        GetEvents(e => e.ModelId == id && e.ModelVersion <= version);

    public IEnumerable<IEvent> GetByTime(Guid id, DateTime until) =>
        GetEvents(e => e.ModelId == id && e.When <= until);

    public IEnumerable<IEvent> GetFromVersion(Guid id, int version) =>
        GetEvents(e => e.ModelId == id && e.ModelVersion > version);

    private IEnumerable<IEvent> GetEvents(Expression<Func<PersistentEvent<string>, bool>> expression) =>
        (_eventStoreContext.Beneficiarios ?? throw new InvalidOperationException()).Where(expression)
            .OrderBy(e => e.ModelVersion)
            .Select(e => JsonConvert.DeserializeObject(e.Data, Type.GetType(e.EventType)))
            .Cast<DomainEvent>();
}