using FluentValidation.Results;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract class Aggregate : IAggregate 
{
    private readonly Queue<IDomainEvent> pendingEvents = new();
    private readonly Queue<IFailedEvent> failureEvents = new();
    private readonly Queue<ISuccessEvent> successEvents = new();
    
    public Guid Id { get; protected set; }
    protected long Version { get; set; }
    protected long NextVersion => Version + 1;
    public IEnumerable<IDomainEvent> PendingEvents => pendingEvents.AsEnumerable();
    public IEnumerable<IFailedEvent> FailedEvents => failureEvents.AsEnumerable();
    public IEnumerable<ISuccessEvent> SuccessEvents => successEvents.AsEnumerable();
    
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

    public virtual void RaiseSuccessEvent()
    {
        var @event = new SuccessEvent(Id, Version);
        successEvents.Enqueue(@event);
    }

    public virtual void RaiseFailedEvent()
    {
        var failureEvent = new FailedEvent(Id, ValidationResult.Errors);
        failureEvents.Enqueue(failureEvent);
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
        failureEvents.Clear();
        successEvents.Clear();
    }

    public bool Invalid => ValidationResult.IsValid is false;

    protected ValidationResult ValidationResult { get; set; } = new();

    protected abstract void Validate();
}