using FluentValidation.Results;

namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public class FailedEvent : IFailedEvent
{
    public Guid Id { get; }
    public Guid? UserId { get; }
    public IDictionary<string, string>? Messages { get; }

    public FailedEvent(Guid id, IEnumerable<ValidationFailure> validationFailures)
    {
        Id = id;

        var messages = validationFailures.ToDictionary(failure => failure.ErrorCode, failure => failure.ErrorMessage);

        Messages = messages;
    }

    public FailedEvent(Guid id, IEnumerable<ValidationFailure> validationFailures, Guid? userId)
        : this(id, validationFailures)
    {
        UserId = userId;
    }
}