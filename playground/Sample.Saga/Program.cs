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

        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<TravelStateMachine, TravelState>()
             .MongoDbRepository(r =>
             {
                 r.Connection = "mongodb://arch-dev-cosmos:PW4MVBKPBBmwHj3v6ibH1NWPfjpToNezcWWF6OPd6Mh2If8PdsxG3jFcdCnYDYhhq0ApDJLgEKuyro2BIPm7LA==@arch-dev-cosmos.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@arch-dev-cosmos@";
                 r.DatabaseName = "saga-db";
                 r.CollectionName = "travel-state";
             });

            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                configurator.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

Console.Title = "Saga Travel";

await host.RunAsync();
