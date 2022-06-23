using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Domain;
using RabbitMQ.Client;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public static class DependencyInjectionNotification
{
    public static IServiceCollection RegisterNotification(this IServiceCollection services, 
        IConfiguration configuration)
    {
        
        services.AddSingleton(configuration.GetSection(nameof(ServiceBusSettings)).Get<ServiceBusSettings>());
        services.AddScoped<IMessageBus, MessageBus>();

        return services;
    }
    
    public static IServiceCollection RegisterNotificationQueue(this IServiceCollection services, 
        IConfiguration configuration, string queueDescricao)
    {
        var settings = configuration.GetSection(nameof(ServiceBusSettings)).Get<ServiceBusSettings>();

        var factory = CreateConnectionFactory(settings);

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: settings.Exchange, type: ExchangeType.Topic);

            var queueName = channel.QueueDeclare(queueDescricao, true, false, false);

            channel.QueueBind(queue: queueName, exchange: settings.Exchange, routingKey: "response");
        }
        
        return services;
    }

    private static ConnectionFactory CreateConnectionFactory(ServiceBusSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            return new ConnectionFactory()
            {
                HostName = settings.HostName,
                UserName = settings.UserName,
                Password = settings.Password,
                Port = settings.Port ?? 5672
            };
        }

        return new ConnectionFactory() { HostName = settings.ConnectionString };
    }

}