using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class MessageBus : IMessageBus
{
    private readonly ServiceBusSettings _settings;
    private readonly ITransactionService _transactionService;

    public MessageBus(ITransactionService transactionService, ServiceBusSettings settings)
    {
        _transactionService = transactionService;
        _settings = settings;
    }
    
    public Task Publish(IEnumerable<IFailureEvent> events)
    {
        var factory = new ConnectionFactory() { HostName = _settings.ConnectionString };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _settings.Exchange,
                                    type: ExchangeType.Topic);

            foreach (var @event in events)
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

                channel.BasicPublish(exchange: _settings.Exchange,
                                     routingKey: $"{_transactionService.GetTransactionId()}.failure.{@event.GetType()}",
                                     basicProperties: null,
                                     body: body);
            }
        }
        return Task.CompletedTask;
    }

    public Task Publish(IEnumerable<IDomainEvent> events)
    {
        var factory = new ConnectionFactory() { HostName = _settings.ConnectionString };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _settings.Exchange,
                                    type: ExchangeType.Topic);

            foreach (var @event in events)
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

                channel.BasicPublish(exchange: _settings.Exchange,
                                     routingKey: $"{_transactionService.GetTransactionId()}.success.{@event.GetType()}",
                                     basicProperties: null,
                                     body: body);
            }
        }
        return Task.CompletedTask;
    }
}