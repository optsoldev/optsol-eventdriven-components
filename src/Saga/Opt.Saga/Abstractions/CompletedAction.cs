using Opt.Saga.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core.Abstractions
{
    public interface ICompletedAction
    {
        Task Execute(SagaContext context, CancellationToken cancellationToken);
    }
    public class PostOnResultSignalR : ICompletedAction
    {
        public async Task Execute(SagaContext context, CancellationToken cancellationToken)
        {
            SignalRHub hub = new SignalRHub();
            var lastOutput = context.Outputs["CriarPlanoSaude"];
            var result = SagaJsonCoverter.DeserializeObject<SagaProcessEventArgs>(lastOutput);
            await hub.SendMessage(context, result);
        }
    }
}
