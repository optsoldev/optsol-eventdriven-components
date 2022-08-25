using FluentValidation.Results;

namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public class FailedEvent : IFailedEvent
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public IEnumerable<string> Messages { get; set; }

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