using System;
using System.Text.Json;
using System.Threading.Tasks;
using EventDriven.Arch.Application.Commands.Commits;
using EventDriven.Arch.Application.Commands.CriarBeneficiarios;
using EventDriven.Arch.Application.Commands.Rollbacks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace EventDriven.Arch.Driving.Beneficiarios;

public class Functions
{
    private readonly IMediator _mediator;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase   
    };
    
    public Functions(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [FunctionName("CriarBeneficiario")]
    [OpenApiOperation(operationId: "CriarBeneficiario", tags: new[] {""}, Description = "Função acionada para criar benefeciário")]
    public async Task CriarBeneficiario([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/beneficiarios/criar")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Enviar Envelope Triggered");
        
        var data = await JsonSerializer.DeserializeAsync<CriarBeneficiarioCommand>(req.Body, _jsonSerializerOptions);

        if (data == null) throw new InvalidCastException("Não foi possível converter para Envelope");

        await _mediator.Send(data);
    }
    
    [FunctionName("CriarDependente")]
    public  Task CriarDependente([HttpTrigger(AuthorizationLevel.Function, "get", 
            Route = "/v1/beneficiarios/{id}/dependentes/criar")] HttpRequest req, 
        ILogger log)
    {
        log.LogInformation("Enviar Envelope Triggered");
        
        //var data = await JsonSerializer.DeserializeAsync<CriarDependenteCommand>(req.Body, _jsonSerializerOptions);

        //if (data == null) throw new InvalidCastException("Não foi possível converter para Envelope");
        throw new NotImplementedException();
    }
    
    [FunctionName("AdicionarEndereco")]
    public Task AdicionarEndereco(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/beneficiarios/{id}/enderecos/adicionar")] HttpRequest req,
        ILogger log)
    {
        //log.LogInformation("Enviar Envelope Triggered");
        
        //var data = await JsonSerializer.DeserializeAsync<EnvelopeViewModel>(req.Body, _jsonSerializerOptions);

        //if (data == null) throw new InvalidCastException("Não foi possível converter para Envelope");
        throw new NotImplementedException();
    }
    
    [FunctionName("Commit")]
    public async Task Commit(
        [ServiceBusTrigger("flight-success", "S1", Connection = "ServiceBusConnection")] Guid integrationId,
        ILogger log)
    {
        log.LogInformation($@"Commit Triggered IntegrationId: {integrationId} ");
        var command = new CommitCommand(integrationId);

       await _mediator.Send(command);
        
    }
    
    [FunctionName("Rollback")]
    public async Task Rollback(
        [ServiceBusTrigger("flight-failed", "S1", Connection = "ServiceBusConnection")] Guid integrationId,
        ILogger log)
    {
        log.LogInformation($"Rollback Triggered IntegrationId : {integrationId}");
        
        var command = new RollbackCommand(integrationId);

        await _mediator.Send(command);
    }
}