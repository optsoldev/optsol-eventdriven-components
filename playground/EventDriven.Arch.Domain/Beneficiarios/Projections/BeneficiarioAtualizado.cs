using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios.Projections;

public class BeneficiarioAtualizado : IProjection
{
    public Guid Id { get; set; }
    public string PrimeiroNome { get; set; }
    public string SegundoNome { get; set; }
    public string NomeCompleto { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public DateTime CreatedDate { get; set; }
}