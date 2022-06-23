using FluentValidation;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios;

public class Beneficiario : Aggregate
{
    public string? PrimeiroNome { get; private set; }
    public string? SegundoNome { get; private set; }

    private readonly IEnumerable<Endereco> _enderecos = new List<Endereco>();
    
    public IReadOnlyCollection<Endereco> Enderecos => _enderecos.ToList();

    public static Beneficiario Create(string primeiroNome, string segundoNome)
    {
        var beneficiario = new Beneficiario(Enumerable.Empty<IDomainEvent>());
        beneficiario.RaiseEvent(new BeneficiarioCriado(primeiroNome, segundoNome));

        beneficiario.Validate();

        return beneficiario;
    }

    
    public Beneficiario(IEnumerable<IDomainEvent> persistentEvents) :base(persistentEvents) 
    {
    }
    
    public void AlterarNome(string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioAlterado(Id, NextVersion, primeiroNome, segundoNome));
    }
    
    protected override void Validate()
    {
        var validation = new BeneficiarioValidator();
        ValidationResult = validation.Validate(this);
        if (Invalid)
            _failureEvents.Enqueue(new BeneficiarioNaoCriado(ValidationResult.Errors.ToString()));
    }

    protected override void Apply(IDomainEvent pendingEvent)
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
        (alterado.Version, alterado.PrimeiroNome, alterado.SegundoNome);

    public sealed class BeneficiarioValidator : AbstractValidator<Beneficiario>
    {
        public BeneficiarioValidator()
        {
            RuleFor(beneficiario => beneficiario.PrimeiroNome)
                .NotEmpty()
                .NotNull()
                .WithMessage("O primeiro nome não pode ser nulo ou vazio.");

            RuleFor(beneficiario => beneficiario.SegundoNome)
                .NotEmpty()
                .NotNull()
                .WithMessage("O segundo nome não pode ser nulo ou vazio.");
            
        }
    }
}

