using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Flight.Core.Application;
using Sample.Flight.Driving.Commands;
using Sample.Flight.Driving.Commands.Consumers;
using Serilog;
using Serilog.Events;
using MediatR;

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
