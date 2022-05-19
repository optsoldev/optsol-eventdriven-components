namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public abstract record DomainEvent(Guid IntegrationId, Guid ModelId, int ModelVersion, DateTime When) : IDomainEvent;