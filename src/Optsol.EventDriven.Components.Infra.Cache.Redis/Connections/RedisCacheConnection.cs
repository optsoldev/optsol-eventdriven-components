﻿using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Infra.Cache.Redis.Configuration;
using StackExchange.Redis;

namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Connections;

public class RedisCacheConnection
{
    private readonly Lazy<ConnectionMultiplexer> connectionMultiplexer;

    private readonly ILogger? logger;

    public RedisCacheConnection(RedisSettings redisSettings, ILoggerFactory logger)
    {
        this.logger = logger.CreateLogger<RedisCacheConnection>();
        this.logger?.LogInformation("Inicializando RedisCacheConnection");

        if (redisSettings is null)
            throw new ArgumentNullException(nameof(redisSettings));

        connectionMultiplexer = new Lazy<ConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisSettings?.ConnectionString));
    }

    public IDatabase GetDatabase()
    {
        logger?.LogInformation($"Método: {nameof(GetDatabase)}() Retorno: IDatabase");

        return connectionMultiplexer.Value.GetDatabase();
    }
}