using Azure.Messaging.EventHubs.Consumer;
using FluentAssertions;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Text.Json;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification.Integration.Tests
{
    public class MessageBusSpec
    {
        [Fact(Skip="")]
        public async Task Publish_To_EventHub_Sucessusfully()
        {
            var eventDataRead = new List<IDomainEvent>();

            var settings = new ServiceBusSettings()
            {
                ConnectionString = "",
                EventHubName = "beneficiario"
            };

            var messageBus = new MessageBus(settings);

            var events = new List<IDomainEvent>();
            var evt = new Event();
            events.Add(evt);
            await messageBus.Publish(Guid.NewGuid(), events);

            
            var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            var consumer = new EventHubConsumerClient(consumerGroup, settings.ConnectionString, $"{settings.EventHubName}-success");

            try
            {
                using var cancellationSource = new CancellationTokenSource();
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(10));

                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(cancellationSource.Token))
                {
                    var @event = JsonSerializer.Deserialize<Event>(partitionEvent.Data.EventBody.ToString());

                    eventDataRead.Add(@event);
                }
            }
            catch(Exception e)
            {
                eventDataRead.Should().NotBeNullOrEmpty();
            }
            finally
            {
                await consumer.CloseAsync();

            }
            
        }
    }

    public class Event : IDomainEvent
    {
        public Guid IntegrationId => Guid.NewGuid();

        public Guid ModelId => Guid.NewGuid();

        public int ModelVersion => 1;

        public DateTime When => DateTime.UtcNow;
    }
}