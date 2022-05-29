using Functions.Worker.ContextAccessor;
using Optsol.EventDriven.Components.Core.Domain;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Optsol.EventDriven.Components.Core.Application
{
    public class TransactionService : ITransactionService
    {
        private readonly IFunctionContextAccessor _accessor;

        public TransactionService(IFunctionContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public Guid GetTransactionId()
        {
            JsonObject headers = JsonSerializer.Deserialize<JsonObject>(_accessor.FunctionContext.BindingContext.BindingData["Headers"].ToString());
            var transaction = headers["Transaction"].ToString();

            return Guid.Parse(transaction);
        }
    }
}
