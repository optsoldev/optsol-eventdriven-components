using Microsoft.AspNetCore.Http;
using Optsol.EventDriven.Components.Core.Domain;

namespace Optsol.EventDriven.Components.Core.Application
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid GetTransactionId()
        {
           return Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["Transaction"].ToString());
        }
    }
}
