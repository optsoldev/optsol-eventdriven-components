using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios;

public abstract record DomainEvent(Guid IntegrationId, Guid ModelId, int ModelVersion, DateTime When) : IEvent;
public record BeneficiarioAlterado(Guid IntegrationId, Guid ModelId, int ModelVersion, string PrimeiroNome, string SegundoNome)
    : DomainEvent(IntegrationId, ModelId, ModelVersion, DateTime.UtcNow);
public record BeneficiarioCriado(Guid IntegrationId, string PrimeiroNome, string SegundoNome)
    : DomainEvent(IntegrationId, Guid.NewGuid(), 1, DateTime.UtcNow);
public record BeneficiarioNaoCriado(Guid IntegrationId, string Reason) : IFailureEvent;