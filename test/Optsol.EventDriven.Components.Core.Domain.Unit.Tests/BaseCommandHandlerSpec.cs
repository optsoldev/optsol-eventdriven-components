using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Core.Domain.Unit.Tests;

public class BaseCommandHandlerSpec
{
    [Fact]
    public void DeveAcionarCommitCasoEntidadeValida()
    {
        var entity = new TesteEntity(Enumerable.Empty<IDomainEvent>());
        
        var logger = new Mock<ILogger<BaseCommandHandler<TesteEntity>>>();

        var mockRepo = new Mock<IWriteEventRepository<TesteEntity>>();

        var handler = new TestCommandHandler(logger.Object, mockRepo.Object, Guid.NewGuid(), entity);
        
        mockRepo.Verify(v => v.Commit(It.IsAny<Guid>(), It.IsAny<TesteEntity>()), Times.Once);
    }
    
    [Fact]
    public void DeveAcionarRollbackCasoEntidadeInvalida()
    {
        var entity = new TesteEntity(Enumerable.Empty<IDomainEvent>());
        
        entity.SetEntityInvalid();
        
        var logger = new Mock<ILogger<BaseCommandHandler<TesteEntity>>>();

        var mockRepo = new Mock<IWriteEventRepository<TesteEntity>>();
        
        var mockNotificator = new Mock<INotificator>();
        
        var handler = new TestCommandHandler(logger.Object, mockRepo.Object , Guid.NewGuid(), entity);
        
        mockRepo.Verify(v => v.Rollback( It.IsAny<TesteEntity>()), Times.Once);
    }

    public class TestCommandHandler : BaseCommandHandler<TesteEntity>
    {
        public TestCommandHandler(
            ILogger<BaseCommandHandler<TesteEntity>> logger, 
            IWriteEventRepository<TesteEntity> writeEventRepository,
            Guid correlationId,
            TesteEntity entity) 
            : base(logger, writeEventRepository)
        {
            //pega entiddade
            //faz o que tem que fazer
            
            SaveChanges(correlationId, entity).Wait();
            
            //return task.
        }
    }

    public record TestFailedEvent(Guid Id, IEnumerable<ValidationFailure>  ValidationFailures) : FailedEvent(Id, ValidationFailures);
    public record TestSuccessEvent(Guid Id, long Version) : SuccessEvent(Id, Version);
   
    public class TesteEntity : Aggregate
    {
        public TesteEntity(IEnumerable<IDomainEvent> persistedEvents) : base(persistedEvents)
        {
        }

        public void SetEntityInvalid()
        {
            ValidationResult = new ValidationResult(new[] {new ValidationFailure("teste", "teste message")});
        }

        protected override void Apply(IDomainEvent @event)
        {
            throw new NotImplementedException();
        }

        protected override void Validate()
        {
        }
    }
}