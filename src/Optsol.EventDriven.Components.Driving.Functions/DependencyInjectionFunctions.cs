using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Optsol.EventDriven.Components.Driving.Functions
{
    public static class DependencyInjectionFunctions
    {
        public static IServiceCollection AddOptsolLogging(this IServiceCollection services)
        {
            services.AddLogging(
                lb => lb.ClearProviders()
                    .AddSerilog(
                        new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateLogger(),
                        true));

            return services;
        }
    }
}
