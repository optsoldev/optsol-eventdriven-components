using FluentValidation.Results;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract class Aggregate : IAggregate 
{
    private readonly Queue<IDomainEvent> pendingEvents = new();

    public Guid Id { get; protected set; }
    public long Version { get; protected set; }
    protected long NextVersion => Version + 1;
    public IEnumerable<IDomainEvent> PendingEvents => pendingEvents.AsEnumerable();

    protected Aggregate(IEnumerable<IDomainEvent> persistedEvents)
    {
        var domainEvents = persistedEvents.ToList();
        if (domainEvents.Any())
        {
            ApplyPersistedEvents(domainEvents);
        }
    }
    
    protected void RaiseEvent<TEvent>(TEvent pendingEvent) where TEvent : IDomainEvent
    {
        pendingEvents.Enqueue(pendingEvent);
        Apply(pendingEvent);
        Version = pendingEvent.ModelVersion;
    }

    protected abstract void Apply(IDomainEvent @event);
    
    private void ApplyPersistedEvents(IEnumerable<IDomainEvent> events)
    {
        foreach (var e in events)
        {
            Apply(e);
            Version = e.ModelVersion;
        }
    }

    public void Clear()
    {
        pendingEvents.Clear();
    }

    public bool Invalid => ValidationResult.IsValid is false;

    public ValidationResult ValidationResult { get; protected set; } = new();

    protected abstract void Validate();
}