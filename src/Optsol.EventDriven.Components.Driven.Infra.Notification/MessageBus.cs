using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class MessageBus : IMessageBus
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSettings _settings;
    
    public MessageBus(ServiceBusSettings settings)
    {
        _settings = settings;
        _client = new ServiceBusClient(settings.ConnectionString);
    }
    
    public async Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events)
    {
        var sender = _client.CreateSender($"{_settings.TopicName}-failed");
        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
        foreach (var @event in events)
        {
            var data = JsonConvert.SerializeObject(@events);
            messageBatch.TryAddMessage(new ServiceBusMessage(data));
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
        }
        finally
        {
            await sender.DisposeAsync();
            await _client.DisposeAsync();
        }
    }

    public async Task Publish(Guid integrationId, IEnumerable<IEvent> events)
    {
        var sender = _client.CreateSender($"{_settings.TopicName}-success");
        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
        foreach (var @event in events)
        {
            var data = JsonConvert.SerializeObject(@events);
            messageBatch.TryAddMessage(new ServiceBusMessage(data));
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
        }
        finally
        {
            await sender.DisposeAsync();
            await _client.DisposeAsync();
        }
        
    }
}