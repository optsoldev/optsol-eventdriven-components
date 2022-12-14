using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Events;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Sample.Flight.Core.Domain;
using Sample.Flight.Core.Domain.Projections;

namespace Sample.Flight.Driven.Infra.Data
{
    public static class RegisterInfraDataModule
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

            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("OptsolConvention", pack, t => true);
            
            BsonClassMap.RegisterClassMap<FlightBookCreated>();
            BsonClassMap.RegisterClassMap<FlightUnbooked>();

            services.AddScoped<IMongoClient>(impl => new MongoClient(impl.GetService<MongoClientSettings>()));
            services.AddScoped<MongoContext>();

            return services;
        }

        private static IServiceCollection AddRepositories(IServiceCollection services)
        {
            //Repositories
            
            //Events
            services.AddScoped<IFlightBookReadRepository, FlightBookReadRepository>();
            services.AddScoped<IFlightBookWriteRepository, FlightBookWriteRepository>();
            
            //Projections
            services.AddScoped<IFlightBookListReadRepository, FlightBookListReadRepository>();
            services.AddScoped<IFlightBookWriteProjectionRepository, FlightBookListWriteRepository>();
            
            
            return services;
        }
    }
}
