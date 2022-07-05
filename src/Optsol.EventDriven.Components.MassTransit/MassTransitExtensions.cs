using MassTransit;
using Microsoft.Extensions.Configuration;
using Optsol.EventDriven.Components.Settings;

public static class MassTransitExtensions {
    public static IBusRegistrationConfigurator UsingRabbitMq(this IBusRegistrationConfigurator bus, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        bus.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.Vhost, h =>
            {
                h.Username(rabbitMqSettings.Username);
                h.Password(rabbitMqSettings.Password);
            });
            configurator.ConfigureEndpoints(context);
        });

        return bus;
    }

    public static ISagaRegistrationConfigurator<TSaga> MongoDbRepository<TSaga>(this ISagaRegistrationConfigurator<TSaga> configurator,
            IConfiguration configuration, string collectionName)
            where TSaga : class, ISagaVersion
    {
        var mongoSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();

        configurator.MongoDbRepository(r =>
         {
             r.Connection = mongoSettings.Connection;
             r.DatabaseName = mongoSettings.DatabaseName;
             r.CollectionName = collectionName;
         });

        return configurator;
    }
}