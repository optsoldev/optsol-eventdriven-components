using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Domain;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public static class DependencyInjectionNotification
{
    public static IServiceCollection RegisterNotification(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(ServiceBusSettings)).Get<ServiceBusSettings>());
        services.AddScoped<IMessageBus, MessageBus>();
        
        return services;
    }
}