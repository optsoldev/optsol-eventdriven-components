using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Projections;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using MongoDB.Driver;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioWriteProjectionRepository : IBeneficiarioWriteProjectionRepository
{
    private readonly MongoContext _context;
    private readonly IMongoCollection<BeneficiarioAtualizado> _set;
    
    public BeneficiarioWriteProjectionRepository(MongoContext context)
    {
        _context = context;
        _set = context.GetCollection<BeneficiarioAtualizado>(nameof(BeneficiarioAtualizado));
    }

    public void ReceiveEvent(IEvent @event)
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
        var beneficiario = _set.Find(f => f.Id == id).FirstOrDefault();

        if (beneficiario == null)
        {
            beneficiario = new BeneficiarioAtualizado()
            {
                Id = id
            };
            _context.AddTransaction(() => _set.InsertOneAsync(beneficiario));
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

        _context.SaveChanges();
    }

    private void Apply(BeneficiarioAlterado alterado)
    {
        var beneficiario = Get(alterado.ModelId);
        beneficiario.PrimeiroNome = alterado.PrimeiroNome;
        beneficiario.SegundoNome = alterado.SegundoNome;
        beneficiario.DataAtualizacao = alterado.When;
        beneficiario.NomeCompleto = alterado.PrimeiroNome + alterado.SegundoNome;

        _context.SaveChanges();
    }
}