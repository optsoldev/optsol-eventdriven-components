using MassTransit;
using Sample.Flight.Core.Application;
using Sample.Flight.Driving.Commands;
using Sample.Flight.Driving.Commands.Consumers;
using Serilog;
using Serilog.Events;
using MediatR;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Notification;
using Sample.Flight.Driven.Infra.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Driven.Settings;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        services.AddDataMongoModule(configuration);
        services.AddScoped<INotificator, Notificator>();
        services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);

        services.AddHostedService<Worker>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddConsumersFromNamespaceContaining(typeof(BookFlightConsumer));

            bus.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqSettings.Host, rabbitMqSettings.Vhost, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

Console.Title = "Flight Consumer";

await host.RunAsync();
