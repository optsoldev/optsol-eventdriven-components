using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
  
    public class SagaEventHandler
    {
        public async Task<SagaProcessEventArgs> Execute(SagaContext context, CancellationToken cancellationToken)
        {
            //var processor = new MessageProcessor(this.GetMessagingRoutingKeys(context));

            CancellationTokenSource cts = new CancellationTokenSource();

            cts.CancelAfter(context.DefaultTimeout);



            var processorTask = await context.MessageProcessor.WaitForResult(context, cts.Token);
            context.LogInformation($"{context.TransactionId} -event received");

            return processorTask;

        }
    }
}
