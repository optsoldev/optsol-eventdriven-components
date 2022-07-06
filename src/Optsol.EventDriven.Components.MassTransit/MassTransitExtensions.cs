using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Settings;

public static class MassTransitExtensions {

    /// <summary>
    /// Extension method to add the use of RabbitMq for MassTransit with RabbitMqSettings configured.
    /// </summary>
    /// <param name="bus">instance of <see cref="IBusRegistrationConfigurator"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="action">(Optional) action for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    public static IBusRegistrationConfigurator UsingRabbitMq(this IBusRegistrationConfigurator bus, 
        IConfiguration configuration, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? action = null)
    {
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        if (rabbitMqSettings == null)
            throw new ArgumentNullException(nameof(RabbitMqSettings), "RabbitMqSettings in AppSettings needed");

        bus.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.Vhost, h =>
            {
                h.Username(rabbitMqSettings.Username);
                h.Password(rabbitMqSettings.Password);
            });

            configurator.ConfigureEndpoints(context);

            action?.Invoke(context, configurator);
        });

        return bus;
    }

    /// <summary>
    /// Extension method to add the use of MassTransit in project with use of Consumer mapped.    
    /// </summary>
    /// <typeparam name="TConsumer">instance of <see cref="IConsumer"/></typeparam>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="configure">(Optional) to add specific parameters like Activies</param>
    /// <returns>instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection RegisterMassTransit<TConsumer>(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? configure = null)
        where TConsumer : IConsumer
    {
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        if (rabbitMqSettings == null)
            throw new ArgumentNullException(nameof(RabbitMqSettings), "RabbitMqSettings in AppSettings needed");

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddConsumersFromNamespaceContaining(typeof(TConsumer));

            bus.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.Vhost, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                configurator.ConfigureEndpoints(context);

            });

            configure?.Invoke(bus);
        });

        return services;
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