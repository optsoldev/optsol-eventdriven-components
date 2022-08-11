namespace Optsol.EventDriven.Components.Core.Domain.Entities.Events;

public record SuccessEvent(Guid Id, long Version): ISuccessEvent;