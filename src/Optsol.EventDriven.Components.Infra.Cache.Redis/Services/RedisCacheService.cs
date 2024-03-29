﻿using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Infra.Cache.Redis.Connections;
using StackExchange.Redis;

namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Services;

public class RedisCacheService : IRedisCacheService
{
    protected readonly ILogger? logger;
    protected readonly IDatabase database;
    private RedisCacheConnection connection;

    public RedisCacheService(RedisCacheConnection connection, ILoggerFactory logger)
    {
        this.logger = logger.CreateLogger(nameof(RedisCacheService));
        this.logger?.LogInformation("Inicializando RedisCacheService");
        this.connection = connection;
        database = connection?.GetDatabase() ?? throw new ArgumentNullException(nameof(connection));

    }

    public Task<T?> ReadAsync<T>(string key) where T : class
    {
        var value = database.StringGet(key);

        if (value.HasValue)
            return Task.FromResult(value.ToString().To<T>());
        else
            return Task.FromResult(default(T));
    }

    public Task SaveAsync<T>(KeyValuePair<string, T> data) where T : class
    {
        database.StringSet(data.Key, data.Value.ToJson());

        return Task.CompletedTask;
    }

    public Task SaveAsync<T>(KeyValuePair<string, T> data, int expirationInMinutes) where T : class
    {
        database.StringSet(data.Key, data.Value.ToJson(), expiry: new TimeSpan(0, expirationInMinutes, 0));

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string key)
    {
        database.KeyDelete(key);

        return Task.CompletedTask;
    }
}