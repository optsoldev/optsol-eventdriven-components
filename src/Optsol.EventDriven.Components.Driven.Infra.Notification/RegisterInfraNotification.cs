using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Domain;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public static class RegisterInfraNotification
{
    public static IServiceCollection RegisterNotification(this IServiceCollection services)
    {
       
        services.AddScoped<INotificator, Notificator>();

        return services;
    }
}