using MassTransit;
using MassTransit.SagaStateMachine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Core.Contracts;
using Optsol.EventDriven.Components.Settings;

namespace Optsol.EventDriven.Components.MassTransit;

public static class MassTransitExtensions
{
    public static EventActivityBinder<TSaga, TData> SendAsync<TSaga, TData, TMessage>(this EventActivityBinder<TSaga, TData> source,
        Type uriName,
        Func<BehaviorContext<TSaga, TData>, Task<SendTuple<TMessage>>> messageFactory,
        ExchangeType exchangeType = ExchangeType.Queue)
        where TSaga : class, SagaStateMachineInstance
        where TData : class
        where TMessage : class
    {
        var destinationAddress = MessageBusUri.GetInstance().CreateUri(uriName, exchangeType);
        
        return source.Add(new SendActivity<TSaga, TData, TMessage>(_ => destinationAddress, MessageFactory<TMessage>.Create(messageFactory)));
    }
    
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
                bus.UsingAzureServiceBus(configuration, new []{actionAzureServiceBus});
                break;
            case MessageBusType.RabbitMq:
                bus.UsingRabbitMq(configuration, new []{actionRabbitMq});
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
    /// <param name="actions">(Optional) action for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    private static IBusRegistrationConfigurator UsingAzureServiceBus(this IBusRegistrationConfigurator bus,
        IConfiguration configuration, Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>[]? actions = null)
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

            if (actions is not null)
            {
                foreach (var action in actions)
                {
                    action?.Invoke(context, configurator);
                }
            }
        });

        return bus;
    }

    /// <summary>
    /// Extension method to add the use of RabbitMq for MassTransit with RabbitMqSettings configured.
    /// </summary>
    /// <param name="bus">instance of <see cref="IBusRegistrationConfigurator"/></param>
    /// <param name="configuration">instance of <see cref="IConfiguration"/></param>
    /// <param name="actions">(Optional) actions for adding specific parameters like ReceiveEndpoints.</param>
    /// <returns>instance of <see cref="IBusRegistrationConfigurator"/></returns>
    private static IBusRegistrationConfigurator UsingRabbitMq(this IBusRegistrationConfigurator bus,
        IConfiguration configuration, Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>?[]? actions = null)
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

            if (actions != null)
                foreach (var action in actions)
                {
                    action?.Invoke(context, configurator);
                }
        });

        return bus;
    }

    public static IServiceCollection RegisterMassTransitWithScheduler<TConsumer>(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configure = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? actionRabbitMq = null,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? actionAzureServiceBus = null)
        where TConsumer : IConsumer
    {
        var actionsRabbitMq = new List<Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>>();
        if(actionRabbitMq is not null) actionsRabbitMq.Add(actionRabbitMq);
        
        actionsRabbitMq.Add((_, cfg) =>
        {
            cfg.UseDelayedMessageScheduler();
        });

        var actionsAzureServiceBus = new List<Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>>();
        if(actionAzureServiceBus is not null) actionsAzureServiceBus.Add(actionAzureServiceBus);

        actionsAzureServiceBus.Add((_, cfg) =>
        {
            cfg.UseDelayedMessageScheduler();
        });

        return RegisterMassTransit<TConsumer>(services, configuration, configure, 
            actionsRabbitMq.ToArray(), 
            actionsAzureServiceBus.ToArray());
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
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>[]? actionRabbitMq = null,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>[]? actionAzureServiceBus = null)
        where TConsumer : IConsumer
    {
        var messageBusSettings = new MessageBusSettings();
        configuration.Bind(nameof(MessageBusSettings), messageBusSettings);

        if (messageBusSettings is null)
            throw new ArgumentNullException(nameof(MessageBusSettings), "MessageBusSettings in AppSettings needed");

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddSingleton(new MessageBusUri(messageBusSettings));
        
        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddConsumers<TConsumer>();

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

        foreach (var consumer in consumers)
        {
            var interfaces = consumer
                .GetInterfaces().Where(x => typeof(IConsumerAddress).IsAssignableFrom(x) && x != typeof(IConsumerAddress)).ToList();

            if (interfaces.Count > 1)
            {
                throw new ArgumentException(
                    $"Consumer: {consumer.Name} must implement only one interface that inherit from {nameof(IConsumerAddress)}");
            }

            if (!interfaces.Any())
            {
                throw new ArgumentException($"Consumer: {consumer.Name} must implement one interface that inherit from {nameof(IConsumerAddress)}");
            }


            var consumerName = MessageBusUri.GetInstance().FormatName(interfaces.Single().Name);
           
            bus.AddConsumer(consumer).Endpoint(
                e => e.Name = consumerName);
        }
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