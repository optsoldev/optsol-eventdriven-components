using System;
using System.Text.Json;
using System.Threading.Tasks;
using EventDriven.Arch.Application.Commands.CriarBeneficiarios;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace EventDriven.Arch.Driving.Beneficiarios;

/// <summary>
/// 
/// </summary>
public class Functions : BaseFunction
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    public Functions(IMediator mediator) : base(mediator) {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <param name="log"></param>
    /// <exception cref="InvalidCastException"></exception>
    [FunctionName("CriarBeneficiario")]
    [OpenApiOperation(operationId: "CriarBeneficiario", tags: new[] {""}, Description = "Função acionada para criar benefeciário")]
    public async Task CriarBeneficiarioAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/beneficiarios/criar")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Criar Beneficiário Triggered");
        
        var data = await JsonSerializer.DeserializeAsync<CriarBeneficiarioCommand>(req.Body, JsonSerializerOptions);

        if (data == null) throw new InvalidCastException($"Não foi possível converter para {nameof(CriarBeneficiarioCommand)}");

        await Mediator.Send(data);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
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
    
    
}