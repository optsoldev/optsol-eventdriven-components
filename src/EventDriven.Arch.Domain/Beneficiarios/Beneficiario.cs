using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios;

public class Beneficiario : Aggregate
{
    public string? PrimeiroNome { get; private set; }
    public string? SegundoNome { get; private set; }

    private readonly IEnumerable<Endereco> _enderecos = new List<Endereco>();
    
    public IReadOnlyCollection<Endereco> Enderecos => _enderecos.ToList();

    public Beneficiario(string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioCriado(primeiroNome, segundoNome));
        
    }
    
    public Beneficiario(IEnumerable<IEvent> persistentEvents) :base(persistentEvents) {}
    
    public void AlterarNome(string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioAlterado(Id, NextVersion, primeiroNome, segundoNome));
    }
    
    public override void Validate()
    {
        _failureEvents.Enqueue(new BeneficiarioNaoCriado(""));
    }

    
    protected override void Apply(IEvent pendingEvent)
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
}

