using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Infra.Cache.Redis.Services;

namespace Optsol.EventDriven.Components.Infra.Cache.Redis.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddRedisodule(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new RedisSettings();
        configuration.Bind(nameof(RedisSettings), settings);

        services.AddSingleton(settings);
        services.AddScoped<IRedisCacheService, RedisCacheService>();

        return services;
    }
}