namespace EventDriven.Arch.Domain.Beneficiarios;

public abstract record DomainEvent(Guid ModelId, int ModelVersion, DateTime When) : IEvent;
public record BeneficiarioAlterado(Guid ModelId, int ModelVersion, string PrimeiroNome, string SegundoNome)
    : DomainEvent(ModelId, ModelVersion, DateTime.UtcNow);
public record BeneficiarioCriado(string PrimeiroNome, string SegundoNome)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);
public record BeneficiarioNaoCriado(Guid IntegrationId) : IFailureEvent;