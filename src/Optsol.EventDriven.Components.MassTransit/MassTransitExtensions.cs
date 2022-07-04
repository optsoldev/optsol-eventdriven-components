using MassTransit;
using Microsoft.Extensions.Configuration;
using Optsol.EventDriven.Components.Settings;

public static class MassTransitExtensions {
    public static IBusRegistrationConfigurator OptsolUsingRabbitMq(this IBusRegistrationConfigurator bus, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        bus.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.Vhost, h =>
            {
                h.Username(rabbitMqSettings.Username);
                h.Password(rabbitMqSettings.Password);
            });
            configurator.ConfigureEndpoints(context);
        });

        return bus;
    }
}