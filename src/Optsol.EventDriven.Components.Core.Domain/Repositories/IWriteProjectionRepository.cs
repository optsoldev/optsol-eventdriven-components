using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Repositories;

<<<<<<< HEAD:src/Optsol.EventDriven.Components.Core.Domain/Repositories/IWriteProjectionRepository.cs
public interface IWriteProjectionRepository<T> where T : IProjection, new()
=======
public interface IWriteReadModelRepository<T> where T : IReadModel, new()
>>>>>>> 53426e8e4cc66e8e5b39c2f80ad296bf56cd321a:src/Optsol.EventDriven.Components.Core.Domain/Repositories/IWriteReadRepository.cs
{
    public void ReceiveEvent(IEvent @event);
}