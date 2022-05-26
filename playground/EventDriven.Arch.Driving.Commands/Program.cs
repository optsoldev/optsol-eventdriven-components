using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);
        services.RegisterNotification(context.Configuration);
        services.AddDataMongoModule(context.Configuration);
        services.AddApplicationModule();
    })
    .Build();

host.Run();