using FluentAssertions;
using MassTransit;
using Optsol.EventDriven.Components.Core.Contracts;

namespace Optsol.EventDriven.Components.Unit.Tests;

public class MassTransitExtensionSpec
{

    [Fact]
    public void DeveBuscarAsInterfacesQueImplementamIUriName()
    {
        var type = typeof(ITeste);
        var consumers = typeof(Teste)
            .Assembly
            .GetTypes()
            .Where(w => type.IsAssignableFrom(w) && w.IsClass)
            .ToArray();

        foreach (var consumer in consumers)
        {
            var interfaces = consumer
                .GetInterfaces().Where(x => typeof(IConsumerAddress).IsAssignableFrom(x) && x != typeof(IConsumerAddress)).ToList();

            if (interfaces.Count > 1)
            {
                throw new ArgumentException(
                    "Todo IConsumer deve implementar apenas uma interface que herda de IUriName");
            }
            
            if (interfaces is null || !interfaces.Any())
            {
                throw new ArgumentException("Todo IConsumer deve implementar uma interface que herda de IUriName");
            }

            var result = interfaces.Single().Name.Replace("I", "").Replace("UriName", "").ToKebabCase();

            result.Should().Be("teste-name");
        }
    }
}

public interface ITeste {}
public class TesteMessage {}
public class Teste : IConsumer<TesteMessage>, ITesteName
{
    public Task Consume(ConsumeContext<TesteMessage> context)
    {
        return Task.CompletedTask;
    }
}
public interface ITesteName : IConsumerAddress {}
