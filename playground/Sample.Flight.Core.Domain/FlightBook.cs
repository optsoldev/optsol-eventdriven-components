using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Sample.Flight.Core.Domain;

public class FlightBook : Aggregate
{
    public Guid UserId { get; private set; }
    public string? From { get; private set; }
    public string? To { get; private set; }

    public FlightBook(IEnumerable<IDomainEvent> persistedEvents) : base(persistedEvents)
    {
    }

    protected override void Apply(IDomainEvent @event)
    {
        throw new NotImplementedException();
    }

    protected override void Validate()
    {
        throw new NotImplementedException();
    }
}
