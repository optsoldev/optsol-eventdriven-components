using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddOptsolMassTransit<TConsumer>(this IServiceCollection services) where TConsumer : IConsumer
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddConsumersFromNamespaceContaining(typeof(TConsumer));

            bus.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}