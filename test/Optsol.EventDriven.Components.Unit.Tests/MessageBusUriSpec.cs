using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Core.Contracts;
using Optsol.EventDriven.Components.MassTransit;
using Optsol.EventDriven.Components.Settings;

namespace Optsol.EventDriven.Components.Unit.Tests;

public class MessageBusUriSpec
{
    [Fact]
    public void DeveConverterEnderecoCorretamenteAPartirDeUmaInterfaceQueImplementaIConsumerAddress()
    {
        var services = new ServiceCollection();
        var myConfiguration = new Dictionary<string, string>
        {
            {"MessageBusSettings:Prefix", "prefix"},
            {"MessageBusSettings:MessageBusType", "RabbitMQ"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

        var messageBusSettings = new MessageBusSettings(); 
        configuration.Bind(nameof(MessageBusSettings), messageBusSettings);
        var messageBusUri = new MessageBusUri(messageBusSettings);

        var result = messageBusUri.CreateUri(typeof(ITesteConsumerAddress));

        result.Should().Be("queue:prefix-teste");
    }
    
    public interface ITesteConsumerAddress : IConsumerAddress {}
}
