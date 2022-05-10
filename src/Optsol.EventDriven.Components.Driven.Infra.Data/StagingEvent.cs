namespace Optsol.EventDriven.Components.Driven.Infra.Data;

public record StagingEvent<T>(Guid IntegrationId, Guid ModelId, int ModelVersion, DateTime When, string? EventType,
    T Data)
{
    public static implicit operator PersistentEvent<T>(StagingEvent<T> staging)
    {
        return new PersistentEvent<T>(staging.ModelId, staging.ModelVersion, staging.When, staging.EventType,
            staging.Data);
    }
}
    