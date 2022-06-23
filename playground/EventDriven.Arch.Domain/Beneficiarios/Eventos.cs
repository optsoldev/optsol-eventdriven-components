using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios;

[JsonObject]
public record BeneficiarioAlterado(Guid Id, long Version, string PrimeiroNome, string SegundoNome)
    : DomainEvent(Id, Version, DateTime.UtcNow);

[JsonObject]
public record BeneficiarioCriado(string PrimeiroNome, string SegundoNome)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);

[JsonObject]
public record BeneficiarioNaoCriado(string Reason) : IFailureEvent;