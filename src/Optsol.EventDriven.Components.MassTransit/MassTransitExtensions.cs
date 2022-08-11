using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Settings;

namespace Optsol.EventDriven.Components.MassTransit;

public static class MassTransitExtensions
{
    public static IBusRegistrationConfigurator UsingMessageBus(this IBusRegistrationConfigurator bus,
        IConfiguration configuration,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? actionAzureServiceBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? actionRabbitMq = null)
    {
        var settings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), settings);

        if (settings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        switch (settings.MessageBusType)
        {
            case MessageBusType.AzureServiceBus:
                bus.UsingAzureServiceBus(configuration, actionAzureServiceBus);
                break;
            case MessageBusType.RabbitMq:
                bus.UsingRabbitMq(configuration, actionRabbitMq);
                break;
            default:
                throw new NotImplementedException($"Message Buss Type: {settings.MessageBusType} not implemented.");
        }

        return bus;
    }

    /// <summary>
    /// Extension method to add the use of AzureServiceBus for MassTransit with RabbitMqSettings configured.
    /// </summary>
    /// <param name="bus">instance of <see cref="IBusRegistrationConfigurator"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="action">(Optional) action for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    private static IBusRegistrationConfigurator UsingAzureServiceBus(this IBusRegistrationConfigurator bus,
        IConfiguration configuration, Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? action = null)
    {
        var settings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), settings);

        if (settings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        if (settings?.AzureServiceBusSettings is null)
            throw new ArgumentNullException(nameof(AzureServiceBusSettings), "AzureServiceBusSettings in MessageBusSettings needed");

        bus.UsingAzureServiceBus((context, configurator) =>
        {
            configurator.Host(settings.AzureServiceBusSettings.ConnectionString);

            configurator.ConfigureEndpoints(context);

            action?.Invoke(context, configurator);
        });

        return bus;
    }

    /// <summary>
    /// Extension method to add the use of RabbitMq for MassTransit with RabbitMqSettings configured.
    /// </summary>
    /// <param name="bus">instance of <see cref="IBusRegistrationConfigurator"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="action">(Optional) action for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    private static IBusRegistrationConfigurator UsingRabbitMq(this IBusRegistrationConfigurator bus,
        IConfiguration configuration, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? action = null)
    {
        var settings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), settings);

        if (settings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        if (settings.RabbitMqSettings is null)
            throw new ArgumentNullException(nameof(RabbitMqSettings), "RabbitMqSettings in MessageBusSettings needed");

        bus.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(settings.RabbitMqSettings.Host, settings.RabbitMqSettings.Vhost, h =>
            {
                h.Username(settings.RabbitMqSettings.Username);
                h.Password(settings.RabbitMqSettings.Password);
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
    public static IServiceCollection RegisterMassTransit<TConsumer>(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configure = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? actionRabbitMq = null,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? actionAzureServiceBus = null)
        where TConsumer : IConsumer
    {
        var messageBusSettings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), messageBusSettings);

        if (messageBusSettings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddConsumersFromNamespaceContaining(typeof(TConsumer));

            switch (messageBusSettings.MessageBusType)
            {
                case MessageBusType.AzureServiceBus:
                    bus.UsingAzureServiceBus(configuration, actionAzureServiceBus);
                    break;
                case MessageBusType.RabbitMq:
                    bus.UsingRabbitMq(configuration, actionRabbitMq);
                    break;
                default:
                    throw new NotImplementedException($"Message Buss Type: {messageBusSettings.MessageBusType} not implemented.");
            }

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

    /// <summary>
    /// Extension method to return Subtypes of an Message.    
    /// </summary>
    /// <example>
    /// class abstract Animal
    /// classe Dog : Animal 
    /// class Duck : Animal
    /// 
    /// Message is of type Animal but send to MassTransit the specific subtype.
    /// </example>
    /// <typeparam name="TMessage">Any type.</typeparam>
    /// <param name="context">instance of <see cref="ConsumeContext"/></param>
    /// <param name="message">instance of any type TMessage</param>
    /// <returns>Task</returns>
    public static Task RespondSubTypeAsync<TMessage>(this ConsumeContext context, TMessage message)
    {
        return Task.FromResult(context.RespondAsync(message, message.GetType()));
    }
}