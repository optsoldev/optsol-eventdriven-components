using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Optsol.EventDriven.Components.Unit.Tests;

public class ProjectionRepositoryInjectionSpec
{

    [Fact]
    public void ShouldWork()
    {
        var services = new ServiceCollection();

        services.AddScoped<ITesteWriteRepository, TesteWriteRepository>();
        services.AddScoped<ITesteWriteRepository, TesteWriteRepository2>();
        
        var provider = services.BuildServiceProvider();
        var result = provider.GetServices(typeof(ITesteWriteRepository));

        result.Should().HaveCount(2);
    }
}

public interface IProjection {}
public interface ITesteProjection : IProjection {}
public interface IWriteRepository<T> where T : IProjection {}
public class WriteRepository<T> : IWriteRepository<T> where T : IProjection {}
public class TesteProjection : ITesteProjection {}

public class TesteProjection2: ITesteProjection {}
public interface ITesteWriteRepository : IWriteRepository<ITesteProjection> {}
public class TesteWriteRepository : WriteRepository<TesteProjection>, ITesteWriteRepository {}
public class TesteWriteRepository2 : WriteRepository<TesteProjection2>, ITesteWriteRepository {}