using System.Linq.Expressions;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Driven.Infra.Data;

public class BeneficiarioReadRepository : IBeneficiarioReadRepository
{
    private readonly EventStoreDbContext _eventStoreContext;

    public BeneficiarioReadRepository(EventStoreDbContext eventStoreContext, IMessageBus messageBus)
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

    private IEnumerable<IEvent> GetEvents(Expression<Func<PersistentEvent, bool>> expression) =>
        _eventStoreContext.Beneficiarios.Where(expression)
            .OrderBy(e => e.ModelVersion)
            .Select(e => JsonConvert.DeserializeObject(e.Data, Type.GetType(e.EventType)))
            .Cast<DomainEvent>();
}