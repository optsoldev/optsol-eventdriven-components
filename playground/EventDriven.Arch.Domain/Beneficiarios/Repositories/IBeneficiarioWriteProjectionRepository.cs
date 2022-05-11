using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace EventDriven.Arch.Domain.Beneficiarios.Repositories;

public interface IWriteProjectionRepository
{
    public void ReceiveEvent(IEvent @event);
}

public interface IBeneficiarioWriteProjectionRepository
{
    public void ReceiveEvent(IEvent @event);
}