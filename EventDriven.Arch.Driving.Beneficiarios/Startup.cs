using EventDriven.Arch.Application;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
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