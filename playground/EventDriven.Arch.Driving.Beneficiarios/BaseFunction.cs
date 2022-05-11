using System;
using System.Text.Json;
using System.Threading.Tasks;
using EventDriven.Arch.Application.Commands.Rollbacks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Application.Commands.Commits;

namespace EventDriven.Arch.Driving.Beneficiarios;

/// <summary>
/// 
/// </summary>
public class BaseFunction
{
    /// <summary>
    /// 
    /// </summary>
    protected IMediator Mediator { get;}
    /// <summary>
    /// 
    /// </summary>
    protected readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase   
    };
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    public BaseFunction(IMediator mediator)
    {
        Mediator = mediator;
    } 
    
    /// <summary>
    /// Função criada para realizar o commit de todos os eventos daquela transação.
    /// </summary>
    /// <param name="integrationId">Guid do Id de integração.</param>
    /// <param name="log">Instancia do <see cref="ILogger"/></param>
    ///  [FunctionName("Commit")]
    /// public async Task CommitAsync(
    ///     [ServiceBusTrigger("%TopicNameSuccess%", "commit", Connection = "ServiceBusConnection")] string integrationId,
    ///   ILogger log)
    ///  {
    ///     log.LogInformation($@"Commit Triggered IntegrationId: {integrationId} ");
    ///    var command = new CommitCommand(Guid.Parse(integrationId));

    ///     await Mediator.Send(command);
    /// }
    
    /// <summary>
    /// Função criada para realizar o rollback de todos os eventos daquela transação. 
    /// </summary>
    /// <param name="integrationId">Guid do Id de integração.</param>
    /// <param name="log">Instância do <see cref="ILogger"/></param>
    /// [FunctionName("Rollback")]
    /// public async Task RollbackAsync(
    ///     [ServiceBusTrigger("%TopicNameFailed%", "rollback", Connection = "ServiceBusConnection")] Guid integrationId,
    ///     ILogger log)
    /// {
    ///     log.LogInformation($"Rollback Triggered IntegrationId : {integrationId}");
        
    ///     var command = new RollbackCommand(integrationId);

    ///     await Mediator.Send(command);
    ///  }
    
    [FunctionName("Exemplo")]
    public async Task ExemploCommit(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/exemplo")] string req,
        ILogger log)
    {
        var commit = new CommitCommand(Guid.NewGuid());

        await Mediator.Send(commit);
    }

}