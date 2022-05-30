using Functions.Worker.ContextAccessor;
using Optsol.EventDriven.Components.Core.Domain;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Optsol.EventDriven.Components.Core.Application
{
    public class AutoCommitService : IAutoCommitService
    {
        public bool IsAutoCommit()
        {
            JsonObject headers = JsonSerializer.Deserialize<JsonObject>(_accessor.FunctionContext.BindingContext.BindingData["Headers"].ToString());
            if(headers.TryGetPropertyValue("AutoCommit", out JsonNode autoCommit))
            {
                return bool.Parse(autoCommit.ToString());
            }

            return false;
        }

        private readonly IFunctionContextAccessor _accessor;

        public AutoCommitService(IFunctionContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
