using EventDriven.Arch.Application;
using EventDriven.Arch.Driven.Infra.Data.MongoDb;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Driven.Infra.Notification;


[assembly: FunctionsStartup(typeof(EventDriven.Arch.Driving.Beneficiarios.Startup))]
namespace EventDriven.Arch.Driving.Beneficiarios
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);
            builder.Services.RegisterNotification();
            //builder.Services.AddDataMongoModule(builder.GetContext().Configuration);
        }
    }
}