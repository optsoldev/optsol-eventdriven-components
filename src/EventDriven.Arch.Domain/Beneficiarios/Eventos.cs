namespace EventDriven.Arch.Domain.Beneficiarios;

public abstract record DomainEvent(Guid ModelId, int ModelVersion, DateTime When) : IEvent;

public record BeneficiarioAlterado(Guid ModelId, int ModelVersion, string PrimeiroNome, string SegundoNome)
    : DomainEvent(ModelId, ModelVersion, DateTime.UtcNow);
public record BeneficiarioCriado(string PrimeiroNome, string SegundoNome)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);


public interface IEvent
{
    public Guid ModelId { get; }
    public int ModelVersion { get; }
    public DateTime When { get; }
}

public interface IIntegrationEvent
{
}

public interface IIntegrationSucessEvent : IIntegrationEvent
{
    //T object
}

public interface IIntegrationFailureEvent : IIntegrationEvent
{
    //Erros
}

public record BeneficiarioCriadoComSucesso(Guid Id) : IIntegrationSucessEvent;

public record BeneficiarioNaoCriado(Guid IntegrationId) : IIntegrationFailureEvent;