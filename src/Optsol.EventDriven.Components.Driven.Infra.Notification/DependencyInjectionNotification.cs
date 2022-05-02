using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Domain;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public static class DependencyInjectionNotification
{
    public static IServiceCollection RegisterNotification(this IServiceCollection services)
    {
        services.AddScoped<IDomainHub, DomainHub>();
        services.AddScoped<IMessageBus, MessageBus>();
        
        return services;
    }
}