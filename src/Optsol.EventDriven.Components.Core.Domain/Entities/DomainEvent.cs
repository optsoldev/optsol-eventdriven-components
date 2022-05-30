using MongoDB.Bson.Serialization.Attributes;

namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract record DomainEvent() : IDomainEvent
{
    [BsonId]
    public Guid ModelId { get; init; }

    public int ModelVersion { get; init; }

    public DateTime When { get; init; }
}