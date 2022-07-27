using Microsoft.Extensions.DependencyInjection;

namespace Optsol.EventDriven.Components.Core.Domain.Unit.Tests;

public class ProjectionRepositoryInjectionSpec
{

    [Fact]
    public void ShouldWork()
    {
        var services = new ServiceCollection();

        services.AddScoped<ITesteWriteRepository, TesteWriteRepository>();
        
        var provider = services.BuildServiceProvider();
        var result = provider.GetServices(typeof(IWriteRepository<>));
    }
}

public interface IProjection {}

public interface ITesteProjection : IProjection {}

public interface IWriteRepository<T> where T : IProjection {}

public class WriteRepository<T> : IWriteRepository<T> where T : IProjection {}
public class TesteProjection : ITesteProjection {}

public interface ITesteWriteRepository : IWriteRepository<TesteProjection> {}


public class TesteWriteRepository : WriteRepository<TesteProjection>, ITesteWriteRepository {}