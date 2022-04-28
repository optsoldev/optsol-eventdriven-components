using EventDriven.Arch.Domain.Beneficiarios;

namespace EventDriven.Arch.Domain;

public interface IBeneficiarioWriteRepository
{
    public void Commit(Guid integrationId, Beneficiario beneficiario);
    public void Rollback(Guid integrationId, Beneficiario beneficiario);
}