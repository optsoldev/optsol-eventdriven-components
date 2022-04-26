using EventDriven.Arch.Domain.Beneficiarios;

namespace EventDriven.Arch.Domain;

public interface IBeneficiarioWriteRepository
{
    public void Commit(Beneficiario beneficiario);
}