using EventDriven.Arch.Domain.Beneficiarios;

namespace EventDriven.Arch.Domain;

public interface IBeneficiarioReadRepository
{
    public IEnumerable<DomainEvent> GetById(Guid id);
}