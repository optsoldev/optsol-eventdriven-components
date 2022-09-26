using System;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Infra.Cache.Redis.Configuration;
using StackExchange.Redis;

namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Connections;

public class RedisCacheConnection
{
    private bool disposed = false;

    private readonly ConnectionMultiplexer connectionMultiplexer;

    private readonly ILogger? logger;

    public RedisCacheConnection(RedisSettings redisSettings, ILoggerFactory logger)
    {
        this.logger = logger.CreateLogger<RedisCacheConnection>();
        this.logger?.LogInformation("Inicializando RedisCacheConnection");

        if (redisSettings is null)
            throw new ArgumentNullException(nameof(redisSettings));

        connectionMultiplexer = ConnectionMultiplexer.Connect(redisSettings?.ConnectionString);
    }

    public IDatabase GetDatabase()
    {
        logger?.LogInformation($"Método: {nameof(GetDatabase)}() Retorno: IDatabase");

        return connectionMultiplexer.GetDatabase();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        logger?.LogInformation($"Método: {nameof(Dispose)}()");

        if (!disposed && disposing)
        {
            connectionMultiplexer.Dispose();
        }

        disposed = true;
    }
}