using EventDriven.Arch.Domain.Beneficiarios;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Logging;

namespace EventDriven.Arch.Driven.Infra.Data;

public class DomainHub : Hub, IDomainHub
{
    private ServiceHubContext _hubContext;

    public DomainHub()
    {
        var serviceManager = new ServiceManagerBuilder().WithOptions(option =>
            {
                option.ConnectionString = "Endpoint=https://signalr-arch.service.signalr.net;AccessKey=p+/OjzeIPjQ5FcQi0elC+bl0KdjlHgnOZJg1v0OkX+k=;Version=1.0;";
                option.ServiceTransportType = ServiceTransportType.Transient;
            })
            //Uncomment the following line to get more logs
            .WithLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .BuildServiceManager();
        
        _hubContext = serviceManager.CreateHubContextAsync("message", default).Result;
    }
    public Task BroadcastSuccess(Guid integrationId, IIntegrationSucessEvent @event)  =>
        _hubContext.Clients.All.SendAsync("sucesso", @event);

    public Task BroadcastFailure(Guid integrationId, IIntegrationFailureEvent @event)  =>
        _hubContext.Clients.All.SendAsync(nameof(BroadcastFailure), @event);
}

public interface IDomainHub
{
    public Task BroadcastSuccess(Guid integrationId, IIntegrationSucessEvent @event);

    public Task BroadcastFailure(Guid integrationId, IIntegrationFailureEvent @event);
}