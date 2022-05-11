using System.Data;
using FluentValidation;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios;

public class Beneficiario : Aggregate
{
    public string? PrimeiroNome { get; private set; }
    public string? SegundoNome { get; private set; }

    private readonly IEnumerable<Endereco> _enderecos = new List<Endereco>();
    
    public IReadOnlyCollection<Endereco> Enderecos => _enderecos.ToList();

    public Beneficiario(Guid integrationId, string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioCriado(integrationId, primeiroNome, segundoNome));
        
    }
    
    public Beneficiario(IEnumerable<IEvent> persistentEvents) :base(persistentEvents) {}
    
    public void AlterarNome(Guid integrationId, string primeiroNome, string segundoNome)
    {
        RaiseEvent(new BeneficiarioAlterado(integrationId,Id, NextVersion, primeiroNome, segundoNome));
    }
    
    public override void Validate(Guid integrationId)
    {
        Validate();
        if(Invalid)
            _failureEvents.Enqueue(new BeneficiarioNaoCriado(integrationId, ValidationResult.Errors.ToString()));
    }

    public override void Validate()
    {
        var validation = new BeneficiarioValidator();
        ValidationResult = validation.Validate(this);
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

