using System.Linq.Expressions;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioReadRepository : IBeneficiarioReadRepository
{
    private readonly MongoContext _context;
    private readonly IMessageBus _messageBus;
    private readonly IMongoCollection<PersistentEvent> _set;

    public BeneficiarioReadRepository(MongoContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
        _set = context.GetCollection<PersistentEvent>(nameof(Beneficiario));
    }

    public IEnumerable<IEvent> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IEvent> GetByVersion(Guid id, int version)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IEvent> GetByTime(Guid id, DateTime until)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IEvent> GetFromVersion(Guid id, int version)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<IEvent> GetEvents(Expression<Func<PersistentEvent, bool>> expression)
    {
        var sortDef = Builders<PersistentEvent>.Sort.Descending(d => d.ModelVersion);

        return _set.Find(expression).Sort(sortDef).Project(p => p.Data).ToList();
    }
}