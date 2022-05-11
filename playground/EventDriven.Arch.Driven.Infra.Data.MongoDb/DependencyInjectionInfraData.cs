using System.Security.Authentication;
using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public static class DependencyInjectionInfraData
{
    public static IServiceCollection AddDataMongoModule(this IServiceCollection services, IConfiguration configuration)
    {
        services = AddContexts(services, configuration);
        services = AddRepositories(services);

        return services;
    }

    private static IServiceCollection AddContexts(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>());
        services.AddSingleton(impl =>
        {
            var mongoSettings = impl.GetService<MongoSettings>() ?? throw new ArgumentNullException(nameof(MongoSettings));

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

    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        //Repositories
        services.AddScoped<IBeneficiarioWriteRepository, BeneficiarioWriteRepository>();
        services.AddScoped<IBeneficiarioReadRepository, BeneficiarioReadRepository>();
        services.AddScoped<IBeneficiarioWriteProjectionRepository, BeneficiarioWriteProjectionRepository>();
        return services;
    }
}
