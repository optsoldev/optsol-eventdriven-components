using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using nJson = Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opt.Saga.Core;
using Opt.Saga.Core.Client.ServiceModel;
using Opt.Saga.Core.Validator;
using Optsol.EventDriven.Components.Core.Domain;
using Serilog.Core;
using System.Dynamic;
using System.Net;
using System.Text;
using System.Text.Json;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Cors;

namespace Opt.Saga.Core.Controllers
{
    [Route("api/saga-client")]
    [ApiController]
    [EnableCors("default")]
    public class SagaClientController : ControllerBase
    {
        private readonly HostsOptions HostsOptions;


        public SagaClientController(ILogger<SagaClientController> logger, JsonSchemaValidator validator, IConfiguration configuration, IOptionsMonitor<Microverse> sagaConfiguration, IOptions<HostsOptions> hostsOptions)
        {
            Logger = logger;
            Validator = validator;
            Configuration = configuration;
            Instance = sagaConfiguration.CurrentValue;
            HostsOptions = hostsOptions.Value;

            sagaConfiguration.OnChange(Listener);
        }
        private void Listener(Microverse settings)
        {
            Instance = settings;
        }

        public ILogger<SagaClientController> Logger { get; }
        public JsonSchemaValidator Validator { get; }
        public IDomainEventConverter DomainEventConverter { get; }
        public IConfiguration Configuration { get; }
        Microverse Instance;
        [HttpPost("start")]
        public async Task<IActionResult> Post(SagaRequest request)
        {
            var transactionId = Guid.NewGuid().ToString();
            try
            {
                var flow = Instance[request.FlowKey];

                var context = new SagaContext(request, transactionId, Logger, Validator, flow.Context.DefaultTimeOut, Configuration, HostsOptions);

                context.StartEventListening();


                await flow
                    .UpdateContext(context)
                    .AddDefaultOutputConverter()
                    .Start();
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                options.Converters.Add(new ObjectConverter());
                var sgresponse = new SagaResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    TransactionId = context.TransactionId,
                    RequestTimespan = DateTime.UtcNow,
                    Success = flow.Status == Respositories.SagaStatus.Success ? true : false,
                    Data = flow.Status == Respositories.SagaStatus.Success ? ToCamelCaseObject(flow.GetOutput().ToString()) : null,
                    Messages = flow.Status == Respositories.SagaStatus.Failure ? flow.GetMessages() : null,
                    ExecutionTime = flow.Context.GetExecutionTime()
                };



                object boolObj = ToCamelCaseObject(JsonSerializer.Serialize(sgresponse));
                return Ok(boolObj);
            }
            catch (Exception e)
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new ObjectConverter());

                Logger.LogCritical($"{e.Message}");

                var sgresponse = new SagaResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    RequestTimespan = DateTime.UtcNow,
                    TransactionId = transactionId,
                    Success = false,
                    Messages = new List<string>() { $"{e.Message} - {e.GetType().Name}" }
                };
                return Ok(System.Text.Json.JsonSerializer.Deserialize<object>(SagaJsonCoverter.SerializeObject(sgresponse), options));
            }
        }
        string ToCamelCaseString(string value)
        {
            var body = JObject.Parse(value);
            body = JObject.FromObject(body.ToObject<ExpandoObject>(), nJson.JsonSerializer.Create(new nJson.JsonSerializerSettings { ContractResolver = new nJson.Serialization.CamelCasePropertyNamesContractResolver() }));
            return body.ToString();
        }
        object ToCamelCaseObject(string value)
        {
            value = ToCamelCaseString(value);
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<object>(value, options);
        }
    }

    public class ObjectConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            // Forward to the JsonElement converter
            var converter = options.GetConverter(typeof(JsonElement)) as System.Text.Json.Serialization.JsonConverter<JsonElement>;
            if (converter != null)
            {
                return converter.Read(ref reader, type, options);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new InvalidOperationException("Directly writing object not supported");
        }
    }
}
