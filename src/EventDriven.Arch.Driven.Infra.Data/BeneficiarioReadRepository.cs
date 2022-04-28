using System.Linq.Expressions;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Newtonsoft.Json;

namespace EventDriven.Arch.Driven.Infra.Data;

public class BeneficiarioReadRepository : IBeneficiarioReadRepository
{
    private readonly EventStoreDbContext _eventStoreContext;

    public BeneficiarioReadRepository(EventStoreDbContext eventStoreContext, IMessageBus messageBus)
    {
        _eventStoreContext = eventStoreContext;
    }
    
    public IEnumerable<DomainEvent> GetById(Guid id) =>
        GetEvents(e => e.ModelId == id);

    public IEnumerable<DomainEvent> GetByVersion(Guid id, int version) =>
        GetEvents(e => e.ModelId == id && e.ModelVersion <= version);

    public IEnumerable<DomainEvent> GetByTime(Guid id, DateTime until) =>
        GetEvents(e => e.ModelId == id && e.When <= until);

    public IEnumerable<DomainEvent> GetFromVersion(Guid id, int version) =>
        GetEvents(e => e.ModelId == id && e.ModelVersion > version);

    private IEnumerable<DomainEvent> GetEvents(Expression<Func<PersistentEvent, bool>> expression) =>
        _eventStoreContext.Beneficiarios.Where(expression)
            .OrderBy(e => e.ModelVersion)
            .Select(e => JsonConvert.DeserializeObject(e.Data, Type.GetType(e.EventType)))
            .Cast<DomainEvent>();
}