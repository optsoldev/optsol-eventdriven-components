using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class MessageBus : IMessageBus
{
    private readonly ServiceBusSettings _settings;
    
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    
    public MessageBus(ServiceBusSettings settings)
    {
        _client = new ServiceBusClient(settings.ConnectionString);
        _sender = _client.CreateSender(settings.TopicName);
    }
    
    public async Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events)
    {
        using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();
        foreach (var @event in events)
        {
            var data = JsonConvert.SerializeObject(@events);
            messageBatch.TryAddMessage(new ServiceBusMessage(data));
        }

        try
        {
            await _sender.SendMessagesAsync(messageBatch);
        }
        finally
        {
            await _sender.DisposeAsync();
            await _client.DisposeAsync();
        }
    }

    public async Task Publish(Guid integrationId, IEnumerable<IEvent> events)
    {
        using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();
        foreach (var @event in events)
        {
            var data = JsonConvert.SerializeObject(@events);
            messageBatch.TryAddMessage(new ServiceBusMessage(data));
        }

        try
        {
            await _sender.SendMessagesAsync(messageBatch);
        }
        finally
        {
            await _sender.DisposeAsync();
            await _client.DisposeAsync();
        }
        
    }
}