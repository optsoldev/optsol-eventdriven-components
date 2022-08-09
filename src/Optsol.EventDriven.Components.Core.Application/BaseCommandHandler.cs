using MediatR;
using Microsoft.Extensions.Logging;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Core.Application;

public abstract class BaseCommandHandler<TEntity, TSuccessEvent, TFailedEvent> where TEntity : IAggregate
where TSuccessEvent : ISuccessEvent 
where TFailedEvent : IFailedEvent
{
    protected readonly ILogger<BaseCommandHandler<TEntity, TSuccessEvent, TFailedEvent>> logger;
    protected readonly IWriteEventRepository<TEntity> writeEventRepository;
    protected readonly INotificator notificator;

    public BaseCommandHandler(
        ILogger<BaseCommandHandler<TEntity, TSuccessEvent, TFailedEvent>> logger, IWriteEventRepository<TEntity> writeEventRepository, INotificator notificator)
    {
        this.logger = logger;
        this.writeEventRepository = writeEventRepository;
        this.notificator = notificator;
    }
    
    protected virtual Task<Unit> HandleValidation(Guid correlationid, TEntity entity)
    {
        if (entity.Invalid)
        {
            writeEventRepository.Rollback(entity);
            var failedEvents = entity.FailedEvents.Cast<FailedEvent>().Cast<TFailedEvent>().ToList();
            notificator.Publish(failedEvents);
           
        }
        else
        {
            writeEventRepository.Commit(correlationid, entity);
            var successEvents = entity.SuccessEvents.Cast<SuccessEvent>().Cast<TSuccessEvent>().ToList();
            notificator.Publish(successEvents);   
        }
        
        entity.Clear();

        return Task.FromResult(new Unit());
    }
}