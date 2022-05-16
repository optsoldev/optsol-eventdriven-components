using EventDriven.Arch.Application;
using EventDriven.Arch.Driven.Infra.Data.MongoDb;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Driven.Infra.Notification;


[assembly: FunctionsStartup(typeof(EventDriven.Arch.Driving.Beneficiarios.Startup))]
namespace EventDriven.Arch.Driving.Beneficiarios
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(ApplicationMediatREntryPoint).Assembly);
            builder.Services.RegisterNotification(builder.GetContext().Configuration);
            builder.Services.AddDataMongoModule(builder.GetContext().Configuration);
            builder.Services.AddApplicationModule();
        }
    }
}