using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Settings;

namespace Optsol.EventDriven.Components.MassTransit;

public static partial class MassTransitExtensions
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
                bus.UsingAzureServiceBus(configuration, false, actions: new[] { actionAzureServiceBus });
                break;
            case MessageBusType.RabbitMq:
                bus.UsingRabbitMq(configuration, false, actions: new[] { actionRabbitMq });
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
    /// <param name="useDelayedMessageScheduler"></param>
    /// <param name="actions">action for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    private static IBusRegistrationConfigurator UsingAzureServiceBus(this IBusRegistrationConfigurator bus,
        IConfiguration configuration, bool useDelayedMessageScheduler,
        params Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>[]? actions)
    {
        var settings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), settings);

        if (settings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        if (settings?.AzureServiceBusSettings is null)
            throw new ArgumentNullException(nameof(AzureServiceBusSettings),
                "AzureServiceBusSettings in MessageBusSettings needed");

        bus.UsingAzureServiceBus((context, configurator) =>
        {
            if (useDelayedMessageScheduler)
                configurator.UseServiceBusMessageScheduler();

            configurator.Host(settings.AzureServiceBusSettings.ConnectionString);
            configurator.ConfigureEndpoints(context);

            if (!actions?.Any() ?? false)
                return;

            foreach (var action in actions)
                action?.Invoke(context, configurator);
        });

        return bus;
    }

    /// <summary>
    /// Extension method to add the use of RabbitMq for MassTransit with RabbitMqSettings configured.
    /// </summary>
    /// <param name="bus">instance of <see cref="IBusRegistrationConfigurator"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="useDelayedMessageScheduler"></param>
    /// <param name="actions">actions for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    private static IBusRegistrationConfigurator UsingRabbitMq(this IBusRegistrationConfigurator bus,
        IConfiguration configuration, bool useDelayedMessageScheduler,
        params Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>[]? actions)
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

            if (useDelayedMessageScheduler)
                configurator.UseDelayedMessageScheduler();

            configurator.ConfigureEndpoints(context);

            if (!(actions?.Any() ?? false)) 
                return;

            foreach (var action in actions)
                action?.Invoke(context, configurator);
        });

        return bus;
    }

    /// <summary>
    /// Register MassTransit with the Scheduler for RabbitMq or AzureServiceBus.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configure"></param>
    /// <param name="actionRabbitMq"></param>
    /// <param name="actionAzureServiceBus"></param>
    /// <typeparam name="TConsumer"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public static IServiceCollection RegisterMassTransitWithScheduler<TConsumer>(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configure = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? actionRabbitMq = null,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? actionAzureServiceBus = null)
        where TConsumer : IConsumer
    {
        var actionsRabbitMq = new List<Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>>();
        if (actionRabbitMq is not null) actionsRabbitMq.Add(actionRabbitMq);

        actionsRabbitMq.Add((_, cfg) => { cfg.UseDelayedMessageScheduler(); });

        var actionsAzureServiceBus = new List<Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>>();
        if (actionAzureServiceBus is not null) actionsAzureServiceBus.Add(actionAzureServiceBus);

        actionsAzureServiceBus.Add((_, cfg) => { cfg.UseDelayedMessageScheduler(); });

        var messageBusSettings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), messageBusSettings);

        if (messageBusSettings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        var kebab = new KebabCaseEndpointNameFormatter(messageBusSettings.Prefix, false);

        services.TryAddSingleton(kebab);

        services.AddSingleton(new MessageBusUri(messageBusSettings));

        services.AddMassTransit(bus =>
        {
            bus.SetEndpointNameFormatter(kebab);

            bus.AddConsumers<TConsumer>();

            switch (messageBusSettings.MessageBusType)
            {
                case MessageBusType.AzureServiceBus:
                    bus.AddServiceBusMessageScheduler();
                    bus.UsingAzureServiceBus(configuration, true, actionsAzureServiceBus.ToArray());
                    break;
                case MessageBusType.RabbitMq:
                    bus.AddDelayedMessageScheduler();
                    bus.UsingRabbitMq(configuration, true, actionsRabbitMq.ToArray());
                    break;
                default:
                    throw new NotImplementedException(
                        $"Message Bus Type: {messageBusSettings.MessageBusType} not implemented.");
            }

            configure?.Invoke(bus);
        });

        return services;
    }

    /// <summary>
    /// Extension method to add the use of MassTransit in project with use of Consumer mapped.    
    /// </summary>
    /// <typeparam name="TConsumer">instance of <see cref="IConsumer"/></typeparam>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="configure">(Optional) to add specific parameters like Activies</param>
    /// <param name="actionRabbitMq">(Optional> aditional parameters to add in rabbitMq configuration.</param>
    /// <param name="actionAzureServiceBus">(Optional) aditional parameters to add in azure service bus configuration.</param>
    /// <returns>instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection RegisterMassTransit<TConsumer>(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configure = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? actionRabbitMq = null,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? actionAzureServiceBus = null)
        where TConsumer : IConsumer
    {
        var actionsRabbitMq = new List<Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>>();
        if (actionRabbitMq is not null) actionsRabbitMq.Add(actionRabbitMq);

        var actionsAzureServiceBus = new List<Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>>();
        if (actionAzureServiceBus is not null) actionsAzureServiceBus.Add(actionAzureServiceBus);

        var messageBusSettings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), messageBusSettings);

        if (messageBusSettings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        var kebab = new KebabCaseEndpointNameFormatter(messageBusSettings.Prefix, false);

        services.TryAddSingleton(kebab);

        services.AddSingleton(new MessageBusUri(messageBusSettings));

        services.AddMassTransit(bus =>
        {
            bus.SetEndpointNameFormatter(kebab);

            bus.AddConsumers<TConsumer>();

            switch (messageBusSettings.MessageBusType)
            {
                case MessageBusType.AzureServiceBus:
                    bus.UsingAzureServiceBus(configuration, useDelayedMessageScheduler: false, actionsAzureServiceBus.ToArray());
                    break;
                case MessageBusType.RabbitMq:
                    bus.UsingRabbitMq(configuration, useDelayedMessageScheduler: false, actionsRabbitMq.ToArray());
                    break;
                default:
                    throw new NotImplementedException(
                        $"Message Bus Type: {messageBusSettings.MessageBusType} not implemented.");
            }

            configure?.Invoke(bus);
        });

        return services;
    }

    /// <summary>
    /// Extension method to add the use of MassTransit in project with use of Consumer mapped.    
    /// </summary>
    /// <param name="services">instance of <see cref="IServiceCollection"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="configure">(Optional) to add specific parameters like Activies</param>
    /// <param name="actionRabbitMq">(Optional> aditional parameters to add in rabbitMq configuration.</param>
    /// <param name="actionAzureServiceBus">(Optional) aditional parameters to add in azure service bus configuration.</param>
    /// <returns>instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection RegisterMassTransit(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configure = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? actionRabbitMq = null,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? actionAzureServiceBus = null)
    {
        var actionsRabbitMq = new List<Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>>();
        if (actionRabbitMq is not null) actionsRabbitMq.Add(actionRabbitMq);

        var actionsAzureServiceBus = new List<Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>>();
        if (actionAzureServiceBus is not null) actionsAzureServiceBus.Add(actionAzureServiceBus);

        var messageBusSettings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), messageBusSettings);

        if (messageBusSettings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        var kebab = new KebabCaseEndpointNameFormatter(messageBusSettings.Prefix, false);

        services.TryAddSingleton(kebab);

        services.AddSingleton(new MessageBusUri(messageBusSettings));

        services.AddMassTransit(bus =>
        {
            bus.SetEndpointNameFormatter(kebab);

            switch (messageBusSettings.MessageBusType)
            {
                case MessageBusType.AzureServiceBus:
                    bus.UsingAzureServiceBus(configuration, useDelayedMessageScheduler: false, actionsAzureServiceBus.ToArray());
                    break;
                case MessageBusType.RabbitMq:
                    bus.UsingRabbitMq(configuration, useDelayedMessageScheduler: false, actionsRabbitMq.ToArray());
                    break;
                default:
                    throw new NotImplementedException(
                        $"Message Bus Type: {messageBusSettings.MessageBusType} not implemented.");
            }

            configure?.Invoke(bus);
        });

        return services;
    }

    /// <summary>
    /// AddConsumers using the IUriName as name definition.
    /// </summary>
    /// <param name="bus">instance of <see cref="IBusRegistrationConfigurator"/></param>
    /// <typeparam name="TConsumer">instance of class that implements <see cref="IConsumer"/></typeparam>
    /// <exception cref="ArgumentException">Instance of <see cref="ArgumentException"/> naming the Consumer that had problems</exception>
    public static void AddConsumers<TConsumer>(this IRegistrationConfigurator bus) where TConsumer : IConsumer
    {
        var typeConsumer = typeof(IConsumer);
        var consumers = typeof(TConsumer)
            .Assembly
            .GetTypes()
            .Where(w => typeConsumer.IsAssignableFrom(w))
            .ToArray();

        bus.AddConsumers(consumers);
    }

    public static ISagaRegistrationConfigurator<TSaga> MongoDbRepository<TSaga>(
        this ISagaRegistrationConfigurator<TSaga> configurator,
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

    /// <summary>
    /// Extension method to Add destination address on AddRequestClient. 
    /// </summary>
    /// <param name="bus"></param>
    /// <param name="settings"></param>
    /// <param name="timeout"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddCustomRequestClient<T>(this IBusRegistrationConfigurator bus,
        MessageBusSettings? settings, RequestTimeout timeout = default)
        where T : class
    {
        var type = typeof(T);
        var attribute = type
            .GetCustomAttributes(typeof(PrefixDestinationAddressAttribute), false)
            .Select(s => s as PrefixDestinationAddressAttribute)
            .FirstOrDefault();

        if (attribute is null || string.IsNullOrWhiteSpace(attribute.Prefix))
            bus.AddRequestClient<T>(timeout);
        else
        {
            var server = "";

            switch (settings?.MessageBusType)
            {
                case MessageBusType.AzureServiceBus:
                    var start = settings?.AzureServiceBusSettings?.ConnectionString?.IndexOf("sb") ?? 0;
                    var end = settings?.AzureServiceBusSettings?.ConnectionString?.IndexOf(";") ?? 0;

                    server = settings?.AzureServiceBusSettings?.ConnectionString?.Substring(start, (end - start));
                    break;
                case MessageBusType.RabbitMq:
                    server = $"rabbbit://{settings?.RabbitMqSettings?.Host}{settings?.RabbitMqSettings?.Vhost}";
                    break;
            }

            var destinationAddress = MessageBusUri.GetInstance()
                .CreateUri(type, $"{server}{attribute.Prefix}", exchangeType: ExchangeType.None);

            bus.AddRequestClient<T>(destinationAddress, timeout);
        }
    }
}