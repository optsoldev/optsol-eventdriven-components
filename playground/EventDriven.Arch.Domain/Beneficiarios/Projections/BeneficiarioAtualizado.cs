namespace EventDriven.Arch.Domain.Beneficiarios.Projections;

public interface IReadModel {}

public class BeneficiarioAtualizado : IReadModel
{
    public Guid Id { get; set; }
    public string PrimeiroNome { get; set; }
    public string SegundoNome { get; set; }
    public string NomeCompleto { get; set; }
    public DateTime DataAtualizacao { get; set; }
}