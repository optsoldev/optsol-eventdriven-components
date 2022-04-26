namespace EventDriven.Arch.Domain.Beneficiarios;

public class Beneficiario
{
    #region EventSourcing
    public readonly Queue<IEvent> _pendingEvents = new();

    public IEnumerable<IEvent> PendingEvents
    {
        get => _pendingEvents.AsEnumerable();
    }
    
    #endregion
    
    public Guid Id { get; private set; }
    public int Version { get; private set; } = 0;

    private int NextVersion
    {
        get => Version + 1;
    }
    
    public string PrimeiroNome { get; private set; }
    public string SegundoNome { get; private set; }

    private readonly IEnumerable<Endereco> _enderecos = new List<Endereco>();

    public IReadOnlyCollection<Endereco> Enderecos => _enderecos.ToList();
    public bool IsInvalid { get; set; }
    public IList<IEvent> Errors { get; set; }


    public Beneficiario(string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioCriado(primeiroNome, segundoNome));
    }

    private void RaiseEvent<TEvent>(TEvent pendingEvent) where TEvent : IEvent
    {
        _pendingEvents.Enqueue(pendingEvent);
        Apply(pendingEvent);
        Version = pendingEvent.ModelVersion;
    }

    private void Apply(IEvent pendingEvent)
    {
        switch (pendingEvent)
        {
            case BeneficiarioCriado criado:
                Apply(criado);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void Apply(BeneficiarioCriado criado) => (Id, Version, PrimeiroNome, SegundoNome) =
        (criado.ModelId, criado.ModelVersion, criado.PrimeiroNome, criado.SegundoNome);
}

public class Endereco
{
    
}

public record BeneficiarioCriado(string PrimeiroNome, string SegundoNome)
    : DomainEvent(Guid.NewGuid(), 1, DateTime.UtcNow);

public abstract record DomainEvent(Guid ModelId, int ModelVersion, DateTime When) : IEvent;

public interface IEvent
{
    public Guid ModelId { get; }
    public int ModelVersion { get; }
    public DateTime When { get; }
}

