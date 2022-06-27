using FluentValidation.Results;

namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract class Aggregate : IAggregate 
{
    private readonly Queue<IDomainEvent> _pendingEvents = new();
    protected readonly Queue<IFailureEvent> _failureEvents = new();

    public Guid Id { get; protected set; }

    protected long Version { get; set; } = 0;
    protected long NextVersion
    {
        get => Version + 1;
    }
    public IEnumerable<IDomainEvent> PendingEvents
    {
        get => _pendingEvents.AsEnumerable();
    }

    public IEnumerable<IFailureEvent> FailureEvents
    {
        get => _failureEvents.AsEnumerable();
    }
    
    public Aggregate(IEnumerable<IDomainEvent> persistedEvents)
    {
        if (persistedEvents.Any())
        {
            ApplyPersistedEvents(persistedEvents);
        }
    }
    
    protected void RaiseEvent<TEvent>(TEvent pendingEvent) where TEvent : IDomainEvent
    {
        _pendingEvents.Enqueue(pendingEvent);
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
    
    public void Commit()
    {
        _pendingEvents.Clear();
        _failureEvents.Clear();
    }

    public void Clear()
    {
        _pendingEvents.Clear();
        _failureEvents.Clear();
    }

    public bool Valid => ValidationResult.IsValid;
    
    public bool Invalid => Valid is false;
    
    public ValidationResult ValidationResult { get; protected set; } = new();


    protected abstract void Validate();
}