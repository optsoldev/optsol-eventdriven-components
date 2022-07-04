using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Driven.Settings;
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
        var mongoSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddHostedService<Worker>();

        services.AddSingleton<IBookingHubNotificator>(
            new BookingHubNotificator(
                context.Configuration.GetValue<string>("Websocket:BookingNotificationHub")));

        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<BookingNotificationConsumer>();

            cfg.AddSagaStateMachine<TravelStateMachine, TravelState>()
            .MongoDbRepository(r =>
             {
                 r.Connection = mongoSettings.Connection;
                 r.DatabaseName = mongoSettings.DatabaseName;
                 r.CollectionName = "travel-state";
             });

            cfg.UsingRabbitMq((context, configurator) =>
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

Console.Title = configuration["Title"] ?? "Saga";

await host.RunAsync();
