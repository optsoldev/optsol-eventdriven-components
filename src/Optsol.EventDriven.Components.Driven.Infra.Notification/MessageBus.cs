using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Text.Json;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class MessageBus : IMessageBus
{
    private EventHubProducerClient? _client;
    private readonly ServiceBusSettings _settings;

    public MessageBus(ServiceBusSettings settings)
    {
        _settings = settings;
    }
    
    public async Task Publish(Guid integrationId, IEnumerable<IFailureEvent> events)
    {
        _client = new EventHubProducerClient(_settings.ConnectionString, 
            $"{_settings.EventHubName.ToString().ToLower()}-failure");

        EventDataBatch eventBatch = await _client.CreateBatchAsync();

        foreach (var @event in events)
        {
            var eventBody = new BinaryData(JsonSerializer.Serialize(@event));
            var eventData = new EventData(eventBody);

            if (!eventBatch.TryAdd(eventData))
            {
                await _client.SendAsync(eventBatch);
                eventBatch = await _client.CreateBatchAsync();
                eventBatch.TryAdd(eventData);
            }
        }
    }

    public async Task Publish(Guid integrationId, IEnumerable<IDomainEvent> events)
    {
        _client = new EventHubProducerClient(_settings.ConnectionString,
            $"{_settings.EventHubName.ToString().ToLower()}-success");        
        try
        {
            

            EventDataBatch eventBatch = await _client.CreateBatchAsync();

            foreach (var @event in events)
            {
                var eventBody = new BinaryData(JsonSerializer.Serialize(@event));
                var eventData = new EventData(eventBody);

                if (!eventBatch.TryAdd(eventData))
                {
                    await _client.SendAsync(eventBatch);
                    eventBatch = await _client.CreateBatchAsync();
                    eventBatch.TryAdd(eventData);
                }
            }
            await _client.SendAsync(eventBatch);
            eventBatch.Dispose();
            await _client.DisposeAsync();
        }
        finally
        {
            await _client.CloseAsync();
        }
    }
}