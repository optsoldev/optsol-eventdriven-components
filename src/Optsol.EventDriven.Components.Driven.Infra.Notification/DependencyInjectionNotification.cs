using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optsol.EventDriven.Components.Core.Domain;
using RabbitMQ.Client;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public static class DependencyInjectionNotification
{
    public static IServiceCollection RegisterNotification(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(ServiceBusSettings)).Get<ServiceBusSettings>());
        services.AddScoped<IMessageBus, MessageBus>();

        return services;
    }
    
    public static IHost RegisterNotificationQueue(this IHost host, string queueDescricao)
    {
        var settings = (ServiceBusSettings)host.Services.GetService(typeof(ServiceBusSettings));

        var factory = new ConnectionFactory() { HostName = settings.ConnectionString };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: settings.Exchange, type: ExchangeType.Topic);

            var queueName = channel.QueueDeclare(queueDescricao, true, false, false);

            channel.QueueBind(queue: queueName, exchange: settings.Exchange, routingKey: "response");
        }

        return host;
    }
    
}