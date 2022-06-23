using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Opt.Saga.Core.Messaging;
using Opt.Saga.Core.Validator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class SagaContext : IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public string CurrentStep { get; set; }

        public SagaRequest ClientRequest { get; set; }
        public IDictionary<string, string> Outputs { get; internal set; }
        public IList<string> Errors = new List<string>();
        public TimeSpan DefaultTimeOut { get; set; }
        public string TransactionId { get; set; }
        public ILogger Logger { get; set; }
        public JsonSchemaValidator Validator { get; }
        public TimeSpan DefaultTimeout { get; }
      
        public void StartChronometer() => this._stopwatch.Start();
        public string GetExecutionTime() => this._stopwatch.Elapsed.ToString();
        public MessageProcessor MessageProcessor { get; set; }

        public SagaHttpClient SagaHttpClient { get; set; }
        public readonly HostsOptions HostsOptions;

        public SagaContext(SagaRequest request, string transactionId, ILogger logger, JsonSchemaValidator validator, TimeSpan defaultTimeout,
            IConfiguration configuration, HostsOptions hostsOptions) : this(logger, validator)
        {
            TransactionId = transactionId;
            DefaultTimeout = defaultTimeout;
            ClientRequest = request;
            HostsOptions = hostsOptions;
            //Repository = new SagaRepository(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
        }
        public SagaContext()
        {
            Outputs = new Dictionary<string, string>();
            SagaHttpClient = new SagaHttpClient();

        }
        public void StartEventListening()
        {
            MessageProcessor = new MessageProcessor(GetMessagingRoutingKeys(), Logger);
            this.MessageProcessor.Register(this);
        }
        public SagaContext(ILogger logger, JsonSchemaValidator validator) : this()
        {
            Outputs = new Dictionary<string, string>();
            Logger = logger;
            Validator = validator;
        }
        public IEnumerable<string> GetMessagingRoutingKeys()
        {
            var topics = new string[2] { "success", "failure" };
            return topics.Select(tp => $"{TransactionId}.{tp}");
        }
        public void AddOutput(string name, object value)
        {
            Outputs.Add(name, SagaJsonCoverter.SerializeObject(value));
        }
        // internal void RegisterHttpClient(ISagaHttpClient client) => HttpClient = client;

        public void LogInformation(string message) => Logger.LogInformation($"TID: {TransactionId} - {message}");

        public bool ValidateRequest(string flowRequestType)
        {
            return Validator.Validate(flowRequestType, this.ClientRequest.ToString(), out Errors);
        }


        public void Dispose()
        {
            Logger.LogInformation($"{TransactionId} - Disposing SagaContext");
            this.MessageProcessor?.Dispose();
            this.SagaHttpClient.Dispose();
        }

        internal void AddError(string message)
        {
            this.Errors.Add(message);
        }
    }
}
