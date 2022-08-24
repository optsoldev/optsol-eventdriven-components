using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Core.Application;

public abstract class BaseCommandHandler<TEntity> where TEntity : IAggregate
{
    protected readonly ILogger<BaseCommandHandler<TEntity>> logger;
    protected readonly IWriteEventRepository<TEntity> writeEventRepository;

    protected BaseCommandHandler(
        ILogger<BaseCommandHandler<TEntity>> logger, 
        IWriteEventRepository<TEntity> writeEventRepository)
    {
        this.logger = logger;
        this.writeEventRepository = writeEventRepository;
    }
    
    /// <summary>
    /// Save changes if entity is valid or rollback otherwise.
    /// </summary>
    /// <param name="correlationid">Guid that represent the transaction.</param>
    /// <param name="entity">instance of <see cref="IAggregate"/></param>
    /// <returns>True if success or False if failure.</returns>
    protected virtual Task<bool> SaveChanges(Guid correlationid, TEntity entity)
    {
        if (entity.Invalid)
        {
            writeEventRepository.Rollback(entity);
        }
        else
        {
            writeEventRepository.Commit(correlationid, entity);
        }
        
        return Task.FromResult(!entity.Invalid);
    }

    /// <summary>
    /// Save changes if entity is valid or rollback otherwise. With Success and Failed events published.
    /// </summary>
    /// <param name="correlationid">Guid that represent the transaction.</param>
    /// <param name="entity">instance of <see cref="IAggregate"/></param>
    /// <param name="userId">Guid that represent user logged</param>
    /// <returns>True if success or False if failure.</returns>
    protected virtual Task<bool> SaveChanges<TSuccessEvent, TFailedEvent>(Guid correlationid, TEntity entity)
        where TSuccessEvent : SuccessEvent
        where TFailedEvent : FailedEvent
    {
        if (entity.Invalid)
        {
            writeEventRepository.Rollback<TFailedEvent>(entity);
        }
        else
        {
            writeEventRepository.Commit<TSuccessEvent>(correlationid, entity);
        }
        
        return Task.FromResult(!entity.Invalid);
    }
}