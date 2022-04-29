using EventDriven.Arch.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventDriven.Arch.Driven.Infra.Data;

public static class DependencyInjectionInfraData
{
    public static IServiceCollection RegisterInfraData(this IServiceCollection services)
    {
        services.AddScoped<IDomainHub, DomainHub>();
        services.AddScoped<IMessageBus, MessageBus>();
        services.AddScoped<IBeneficiarioWriteRepository, BeneficiarioWriteRepository>();
        services.AddScoped<IBeneficiarioReadRepository, BeneficiarioReadRepository>();
        services.AddSingleton(new EventStoreDbContext(new DbContextOptionsBuilder<EventStoreDbContext>()
            .UseInMemoryDatabase(databaseName: "EventStore")
            .EnableSensitiveDataLogging()
            .Options));

        return services;
    }
}