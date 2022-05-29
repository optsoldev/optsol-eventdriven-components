using Functions.Worker.ContextAccessor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optsol.EventDriven.Components.Core.Domain;

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
            JObject headers = JsonConvert.DeserializeObject<JObject>(_accessor.FunctionContext.BindingContext.BindingData["Headers"].ToString());
            var transaction = headers["Transaction"].ToString();

            return Guid.Parse(transaction);
        }
    }
}
