using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.MassTransit;
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
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .AddJsonFile("appsettings.json")
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddHostedService<Worker>();

        services.AddSingleton<IBookingHubNotificator>(
            new BookingHubNotificator(
                context.Configuration.GetValue<string>("Websocket:BookingNotificationHub")));

        services.RegisterMassTransit<BookingNotificationConsumer>(configuration, bus =>
        {
            bus.AddSagaStateMachine<TravelStateMachine, TravelState>()
                .MongoDbRepository(configuration, "travel-state");
        });
    })
    .Build();

Console.Title = configuration["Title"] ?? "Saga";

await host.RunAsync();
