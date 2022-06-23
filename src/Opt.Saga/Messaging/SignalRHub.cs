using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core.Messaging
{
    [HubName("sagahub")]
    public class SignalRHub : Hub
    {
        public SignalRHub():base()
        {
        }
        public async Task SendMessage(SagaContext context, SagaProcessEventArgs @event)
        => await Clients.Client(context.TransactionId).SendAsync("SagaCompleted", @event);
    }
}
