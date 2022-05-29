using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios;

public record BeneficiarioAlterado(Guid ModelId, int ModelVersion, string PrimeiroNome, string SegundoNome)
    : DomainEvent(ModelId, ModelVersion, DateTime.UtcNow);
public record BeneficiarioCriado(string PrimeiroNome, string SegundoNome)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);

public record BeneficiarioNaoCriado(string Reason) : IFailureEvent;