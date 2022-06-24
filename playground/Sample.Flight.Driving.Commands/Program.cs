using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Flight.Core.Application;
using Sample.Flight.Driving.Commands;
using Sample.Flight.Driving.Commands.Consumers;
using Serilog;
using Serilog.Events;
using MediatR;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Notification;
using Sample.Flight.Driven.Infra.Data;

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
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddHostedService<Worker>();

        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<BookFlightConsumer>();
            bus.SetKebabCaseEndpointNameFormatter();

            bus.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);
    })
    .Build();

Console.Title = "Flight Consumer";

await host.RunAsync();
