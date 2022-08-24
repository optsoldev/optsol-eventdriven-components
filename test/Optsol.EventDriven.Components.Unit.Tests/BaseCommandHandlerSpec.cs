using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Optsol.EventDriven.Components.Core.Application;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;
using Optsol.EventDriven.Components.Core.Domain.Repositories;

namespace Optsol.EventDriven.Components.Unit.Tests;

public class BaseCommandHandlerSpec
{
    [Fact]
    public void DeveAcionarCommitCasoEntidadeValida()
    {
        var entity = new TesteEntity(Enumerable.Empty<IDomainEvent>());
        
        var logger = new Mock<ILogger<BaseCommandHandler<TesteEntity>>>();

        var mockRepo = new Mock<IWriteEventRepository<TesteEntity>>();

        var handler = new TestCommandHandler(logger.Object, mockRepo.Object, Guid.NewGuid(), entity);
        
        mockRepo.Verify(v => v.Commit<SuccessEvent>(It.IsAny<Guid>(), It.IsAny<TesteEntity>()), Times.Once);
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
        
        mockRepo.Verify(v => v.Rollback<FailedEvent>( It.IsAny<TesteEntity>()), Times.Once);
    }

    [Fact]
    public void DeveCriarFailedEvent()
    {
        var entity = new TesteEntity(Enumerable.Empty<IDomainEvent>());
        
        entity.SetEntityInvalid();

        var failedEvent = new FailedEvent(entity.Id, entity.ValidationResult.Errors);

        failedEvent.Should().NotBeNull();
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
            
            SaveChanges<SuccessEvent, FailedEvent>(correlationId, entity).Wait();
            
            //return task.
        }
    }


    public class TestFailedEvent : FailedEvent
    {
        public TestFailedEvent(Guid Id, IEnumerable<ValidationFailure> ValidationFailures)
        : base(Id,  ValidationFailures){}
    }

    public class TestSuccessEvent : SuccessEvent
    {
        public TestSuccessEvent(Guid Id, long Version) : base(Id, Version) {}
    }
   
    public class TesteEntity : Aggregate
    {
        public TesteEntity(IEnumerable<IDomainEvent> persistedEvents) : base(persistedEvents)
        {
        }

        public void SetEntityInvalid()
        {
            var validationFailure = new ValidationFailure("teste", "teste message");
            validationFailure.ErrorCode = "Teste";
            
            ValidationResult = 
                new ValidationResult(new[] {validationFailure});
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