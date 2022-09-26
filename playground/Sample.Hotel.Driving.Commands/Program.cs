using System.Reflection;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Optsol.EventDriven.Components.Driven.Infra.Notification;
using Optsol.EventDriven.Components.MassTransit;
using Sample.Hotel.Core.Application;
using Sample.Hotel.Driving.Commands;
using Sample.Hotel.Driving.Commands.Consumers;
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
        services.RegisterNotification();

        services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddHostedService<Worker>();

        services.RegisterMassTransit<BookHotelConsumer>(configuration);
    })
    .Build();

Console.Title = "Hotel Consumer";
await host.RunAsync();