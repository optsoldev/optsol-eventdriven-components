using EventDriven.Arch.Application;
using EventDriven.Arch.Driven.Infra.Data.MongoDb;
using Functions.Worker.ContextAccessor;
using MediatR;
using Microsoft.Extensions.Hosting;
using Optsol.EventDriven.Components.Driven.Infra.Notification;
using Optsol.EventDriven.Components.Driving.Functions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseFunctionContextAccessor();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);
        services.RegisterNotification(context.Configuration);
        services.AddDataMongoModule(context.Configuration);
        services.AddApplicationModule();
        services.AddFunctionContextAccessor();
        services.AddOptsolLogging();
    })
    .Build()
    .RegisterNotificationQueue("saga-response-projection");

host.Run();

