using FluentValidation.Results;

namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public class FailedEvent : IFailedEvent
{
    public Guid Id { get; }
    public Guid? UserId { get; }
    public IEnumerable<string> Messages { get; }

    public FailedEvent() {}
    
    public FailedEvent(Guid id, IEnumerable<ValidationFailure> validationFailures)
    {
        Id = id;

        var messages = validationFailures.Select(failure => failure.ErrorMessage);

        Messages = messages;
    }

    public FailedEvent(Guid id, IEnumerable<ValidationFailure> validationFailures, Guid? userId)
        : this(id, validationFailures)
    {
        UserId = userId;
    }
}