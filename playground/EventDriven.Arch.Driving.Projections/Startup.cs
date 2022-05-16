using EventDriven.Arch.Application;
using EventDriven.Arch.Driven.Infra.Data.MongoDb;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EventDriven.Arch.Driving.Projections.Startup))]
namespace EventDriven.Arch.Driving.Projections
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
            builder.Services.AddDataMongoModule(builder.GetContext().Configuration);
            builder.Services.AddApplicationModule();
        }
    }
}