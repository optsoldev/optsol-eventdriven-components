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
        services.AddDataMongoModule(configuration);
        services.AddScoped<INotificator, Notificator>();
        services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);

        services.AddHostedService<Worker>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddConsumersFromNamespaceContaining(typeof(BookFlightConsumer));

            bus.UsingRabbitMq(configuration);
        });
    })
    .Build();

Console.Title = "Flight Consumer";

await host.RunAsync();
