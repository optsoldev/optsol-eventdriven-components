namespace EventDriven.Arch.Domain.Beneficiarios;

public class Beneficiario
{
    #region EventSourcing
    public readonly Queue<IEvent> _pendingEvents = new();
    public readonly Queue<IIntegrationSucessEvent> _pendingIntegrationEvents = new();
    public readonly Queue<IIntegrationFailureEvent> _pedingIntegrationFailureEvents = new();
    public IEnumerable<IEvent> PendingEvents
    {
        get => _pendingEvents.AsEnumerable();
    }

    public IEnumerable<IIntegrationFailureEvent> PedingIntegrationFailureEvents
    {
        get => _pedingIntegrationFailureEvents.AsEnumerable();
    }
    
    public IEnumerable<IIntegrationSucessEvent> PendingIntegrationSuccessEvents
    {
        get => _pendingIntegrationEvents.AsEnumerable();
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

    public Beneficiario(string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioCriado(primeiroNome, segundoNome));
        _pendingIntegrationEvents.Enqueue(new BeneficiarioCriadoComSucesso(Id));
    }

    public Beneficiario(IEnumerable<IEvent> persistedEvents)
    {
        if (persistedEvents.Any())
        {
            ApplyPersistedEvents(persistedEvents);
        }
    }
    private void ApplyPersistedEvents(IEnumerable<IEvent> events)
    {
        foreach (var e in events)
        {
            Apply(e);
            Version = e.ModelVersion;
        }
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
            case BeneficiarioAlterado alterado:
                Apply(alterado);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void Apply(BeneficiarioCriado criado) => (Id, Version, PrimeiroNome, SegundoNome) =
        (criado.ModelId, criado.ModelVersion, criado.PrimeiroNome, criado.SegundoNome);

    private void Apply(BeneficiarioAlterado alterado) => (Version, PrimeiroNome, SegundoNome) =
        (alterado.ModelVersion, alterado.PrimeiroNome, alterado.SegundoNome);
    
    public void Commit()
    {
        _pendingEvents.Clear();
        _pendingIntegrationEvents.Clear();
    }

    public void AlterarNome(string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioAlterado(Id, NextVersion, primeiroNome, segundoNome));
    }
}

