using FluentValidation.Results;

namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public record FailedEvent : IFailedEvent
{
    public FailedEvent(Guid id, IEnumerable<ValidationFailure> validationFailures)
    {
        Id = id;
        Messages = (IDictionary<string, string>?) validationFailures.Select(s => new KeyValuePair<string, string>(s.ErrorCode, s.ErrorMessage));
    }
    public Guid Id { get; }
    public IDictionary<string, string>? Messages { get; }
}