using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Saga;
using Sample.Saga.Components;
using Serilog;
using Serilog.Events;

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
    .ConfigureServices((context, services) =>
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        
        services.AddHostedService<Worker>();

        services.AddMassTransit(bus =>
        {
            bus.AddSagaStateMachine<TravelStateMachine, TravelState>()
            .InMemoryRepository();

            bus.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

Console.Title = "Saga Travel";

await host.RunAsync();
