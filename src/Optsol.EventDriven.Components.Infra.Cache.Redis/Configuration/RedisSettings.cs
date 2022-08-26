namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Configuration;

public class RedisSettings : BaseSettings
{
    public string? ConnectionString { get; set; }

    public override void Validate()
    {
        if (ConnectionString.IsNullOrWhiteSpace())
            ShowingException(nameof(ConnectionString));
    }
}
