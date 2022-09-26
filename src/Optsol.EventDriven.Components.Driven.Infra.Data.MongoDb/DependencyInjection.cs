using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>());
        services.AddSingleton(impl =>
        {
            var mongoSettings = impl.GetService<MongoSettings>() ??
                                throw new ArgumentNullException(nameof(MongoSettings));

            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoSettings.ConnectionString));

            if (settings.UseTls)
            {
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            }

            return settings;
        });

        services.AddScoped<IMongoClient>(impl => new MongoClient(impl.GetService<MongoClientSettings>()));
        services.AddScoped<MongoContext>();
        
        return services;
    }
}