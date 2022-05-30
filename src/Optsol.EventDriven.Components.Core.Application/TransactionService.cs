using Functions.Worker.ContextAccessor;
using Optsol.EventDriven.Components.Core.Domain;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Optsol.EventDriven.Components.Core.Application
{
    public class TransactionService : ITransactionService
    {
        private readonly IFunctionContextAccessor _accessor;
        private readonly JsonObject _headers;

        public TransactionService(IFunctionContextAccessor accessor)
        {
            _accessor = accessor;
            _headers = JsonSerializer.Deserialize<JsonObject>(_accessor.FunctionContext.BindingContext.BindingData["Headers"].ToString());
        }
        public Guid GetTransactionId()
        {
            
            var transaction = _headers["Transaction"].ToString();

            return Guid.Parse(transaction);
        }

        public bool IsAutoCommit()
        {
            if (_headers.TryGetPropertyValue("AutoCommit", out JsonNode? autoCommit))
            {
                return bool.Parse(autoCommit.ToString());
            }

            return false;
        }
    }
}
