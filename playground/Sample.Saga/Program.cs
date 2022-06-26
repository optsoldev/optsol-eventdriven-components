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
        var mongoSettings = configuration.GetSection(nameof(SagaMongoSettings)).Get<SagaMongoSettings>();
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddHostedService<Worker>();

        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<TravelStateMachine, TravelState>()
             .MongoDbRepository(r =>
             {
                 r.Connection = mongoSettings.Connection; ;
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
