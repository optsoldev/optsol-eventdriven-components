using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Domain;

namespace EventDriven.Arch.Driven.Infra.Data.InMemory;

public static class DependencyInjectionInfraData
{
    public static IServiceCollection RegisterInfraData(this IServiceCollection services)
    {
        services.AddScoped<IBeneficiarioWriteRepository, BeneficiarioWriteRepository>();
        services.AddScoped<IBeneficiarioReadRepository, BeneficiarioReadRepository>();
        services.AddSingleton(new EventStoreDbContext(new DbContextOptionsBuilder<EventStoreDbContext>()
            .UseInMemoryDatabase(databaseName: "EventStore")
            .EnableSensitiveDataLogging()
            .Options));

        return services;
    }
}