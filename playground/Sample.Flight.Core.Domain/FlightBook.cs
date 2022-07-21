using FluentValidation;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Sample.Flight.Core.Domain;

public class FlightBook : Aggregate
{
    public Guid UserId { get; private set; }
    public string From { get; private set; }
    public string To { get; private set; }
    public bool Canceled { get; private set; }

    public static FlightBook Create(Guid userId, string from, string to)
    {
        var flightBook = new FlightBook(Enumerable.Empty<IDomainEvent>());

        flightBook.RaiseEvent(new FlightBookCreated(userId, from, to));

        flightBook.Validate();

        return flightBook;
    }

    public FlightBook(IEnumerable<IDomainEvent> persistedEvents) : base(persistedEvents)
    {
    }

    public void Unbook()
    {
        RaiseEvent(new FlightUnbooked(Id, NextVersion));

        Validate();
    }

    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case FlightBookCreated created:
                Apply(created);
                break;                
            case FlightUnbooked unbooked:
                Apply(unbooked);            
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void Apply(FlightUnbooked unbooked) => (Id, Version, Canceled) =
        (unbooked.ModelId, unbooked.ModelVersion, true);

    private void Apply(FlightBookCreated criado) => (Id, Version, UserId, From, To, Canceled) =
        (criado.ModelId, criado.ModelVersion, criado.UserId, criado.From, criado.To, false);

    protected override void Validate()
    {
        var validation = new FlightBookValidator();
        ValidationResult = validation.Validate(this);
    }
}

public sealed class FlightBookValidator : AbstractValidator<FlightBook>
{
    public FlightBookValidator()
    {

    }
}