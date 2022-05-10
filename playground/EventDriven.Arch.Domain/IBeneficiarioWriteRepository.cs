using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace EventDriven.Arch.Domain;

public interface IBeneficiarioWriteRepository : IWriteRepository<Beneficiario> { }