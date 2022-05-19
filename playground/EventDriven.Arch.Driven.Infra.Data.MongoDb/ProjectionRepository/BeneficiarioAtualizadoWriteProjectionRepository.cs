using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Projections;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb.ReadModelRepository;

public class BeneficiarioAtualizadoWriteProjectionRepository : WriteProjectionRepository<BeneficiarioAtualizado>, IBeneficiarioAtualizadoWriteProjectionRepository
{    
    public BeneficiarioAtualizadoWriteProjectionRepository(MongoContext context) : base(context) {} 

    public override void ReceiveEvent(IDomainEvent @event)
    {
        switch (@event)
        {
            case BeneficiarioCriado criado:
                Apply(criado);
                break;
            case BeneficiarioAlterado alterado:
                Apply(alterado);
                break;
        }
    }

    private BeneficiarioAtualizado Get(Guid id)
    {
        var beneficiario = Set.Find(f => f.Id == id).FirstOrDefault();

        if (beneficiario == null)
        {
            beneficiario = new BeneficiarioAtualizado()
            {
                Id = id
            };
            Context.AddTransaction(() => Set.InsertOneAsync(beneficiario));
        }

        return beneficiario;
    }

    private void Apply(BeneficiarioCriado criado)
    {
        var beneficiario = Get(criado.ModelId);
        beneficiario.PrimeiroNome = criado.PrimeiroNome;
        beneficiario.SegundoNome = criado.SegundoNome;
        beneficiario.DataAtualizacao = criado.When;
        beneficiario.NomeCompleto = criado.PrimeiroNome + criado.SegundoNome;

        Context.SaveChanges();
    }

    private void Apply(BeneficiarioAlterado alterado)
    {
        var beneficiario = Get(alterado.ModelId);
        beneficiario.PrimeiroNome = alterado.PrimeiroNome;
        beneficiario.SegundoNome = alterado.SegundoNome;
        beneficiario.DataAtualizacao = alterado.When;
        beneficiario.NomeCompleto = alterado.PrimeiroNome + alterado.SegundoNome;

        Context.SaveChanges();
    }
}