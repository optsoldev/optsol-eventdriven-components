using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core.Messaging
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string EventHubName { get; set; }
    }

    public interface IMessageProcessor
    {
        void Register(SagaContext context);
    }
    public class MessageProcessor : IMessageProcessor, IDisposable
    {
        public IEnumerable<string> RoutingKeys { get; }
        public ILogger Logger { get; }

        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public event OperationHandler OnCompleted = delegate { };
        public MessageProcessor(IEnumerable<string> routingKeys, ILogger logger)
        {
            RoutingKeys = routingKeys;
            Logger = logger;
            this.factory = new ConnectionFactory() {  UserName = "test", Password = "test" , HostName = "20.226.45.207", };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }
        public void Register(SagaContext context)
        {
            channel.ExchangeDeclare(exchange: "planosaude", type: "topic");

            var queueName = channel.QueueDeclare(context.TransactionId, true, false, true);

            foreach (var bindingKey in RoutingKeys)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "planosaude",
                                  routingKey: bindingKey);
            }

            context.LogInformation("Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                string eventType = routingKey.Split(".")[1];

                var @event = new SagaProcessEventArgs(eventType == "failure" ? SagaEventResultType.Failure : SagaEventResultType.Success, Encoding.UTF8.GetString(body));
                context.LogInformation("message received" + message);
                InMemoryStorageService.Add($"{context.CurrentStep}|{context.TransactionId}", @event);
                OnCompleted(this, @event);

                // channel.QueueDelete(queueName);
            };
            channel.BasicConsume(queue: queueName,
                                  autoAck: true,
                                  consumer: consumer);
        }
        public async Task<SagaProcessEventArgs> WaitForResult(SagaContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            if (!InMemoryStorageService.Contains($"{context.CurrentStep}|{context.TransactionId}"))
            {
                try
                {
                    var ob = Observable
                             .FromEventPattern<OperationHandler, SagaProcessEventArgs>(
                                 h => this.OnCompleted += h,
                                 h => this.OnCompleted -= h)

                            .Select(e => e.EventArgs).FirstAsync();
                    var result = await ob;
                    return result;
                }
                catch (TimeoutException)
                {

                    return new SagaProcessEventArgs(SagaEventResultType.Failure, $"process with transactionId {context.TransactionId} - timedout");
                }
            }
            var @event = InMemoryStorageService.Get($"{context.CurrentStep}|{context.TransactionId}");

            return @event as SagaProcessEventArgs;
        }

        public void Dispose()
        {
            Logger.LogInformation($"Disposing MessageProcessor");

            this.connection.Dispose();
            Logger.LogInformation($"connection closed");

            this.channel.Dispose();
            Logger.LogInformation($"channel closed");

        }
    }

}
