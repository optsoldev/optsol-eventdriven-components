using EventDriven.Arch.Application;
using EventDriven.Arch.Driven.Infra.Data.MongoDb;
using Functions.Worker.ContextAccessor;
using MediatR;
using Microsoft.Extensions.Hosting;
using Optsol.EventDriven.Components.Driven.Infra.Notification;

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

    })
    .Build();

host.Run();

