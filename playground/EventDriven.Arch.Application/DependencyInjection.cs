using EventDriven.Arch.Domain.Beneficiarios;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            var register = new DomainEventRegister();
            register.Register(typeof(BeneficiarioCriado));
            register.Register(typeof(BeneficiarioAlterado));

            services.AddSingleton<IDomainEventRegister>(register);
            services.AddSingleton<IDomainEventConverter, DomainEventConverter>();
            services.AddScoped<ITransactionService, TransactionService>();
            return services;
        }
    }
}
