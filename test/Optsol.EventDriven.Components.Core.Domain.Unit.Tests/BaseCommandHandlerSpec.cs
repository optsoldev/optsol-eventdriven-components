using FluentAssertions;
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
        
        var logger = new Mock<ILogger<BaseCommandHandler<TesteEntity, TestSuccessEvent, TestFailedEvent>>>();

        var mockRepo = new Mock<IWriteEventRepository<TesteEntity>>();

        var mockNotificator = new Mock<INotificator>();
        
        var handler = new TestCommandHandler(logger.Object, mockRepo.Object , mockNotificator.Object, Guid.NewGuid(), entity);
        
        mockRepo.Verify(v => v.Commit(It.IsAny<Guid>(), It.IsAny<TesteEntity>()), Times.Once);
    }
    
    [Fact]
    public void DeveAcionarRollbackCasoEntidadeInvalida()
    {
        var entity = new TesteEntity(Enumerable.Empty<IDomainEvent>());
        
        entity.SetEntityInvalid();
        
        var logger = new Mock<ILogger<BaseCommandHandler<TesteEntity, TestSuccessEvent, TestFailedEvent>>>();

        var mockRepo = new Mock<IWriteEventRepository<TesteEntity>>();
        
        var mockNotificator = new Mock<INotificator>();
        
        var handler = new TestCommandHandler(logger.Object, mockRepo.Object , mockNotificator.Object, Guid.NewGuid(), entity);
        
        mockRepo.Verify(v => v.Rollback( It.IsAny<TesteEntity>()), Times.Once);
    }

    public class TestCommandHandler : BaseCommandHandler<TesteEntity, TestSuccessEvent, TestFailedEvent>
    {
        public TestCommandHandler(
            ILogger<BaseCommandHandler<TesteEntity, TestSuccessEvent, TestFailedEvent>> logger, 
            IWriteEventRepository<TesteEntity> writeEventRepository, 
            INotificator notificator,
            Guid correlationId,
            TesteEntity entity) 
            : base(logger, writeEventRepository, notificator)
        {
            HandleValidation(correlationId, entity).Wait();
        }
    }

    public class TestFailedEvent : IFailedEvent
    {
        public TestFailedEvent(Guid id, IDictionary<string, string>? messages)
        {
            Id = id;
            Messages = messages;
        }

        public Guid Id { get; }
        public IDictionary<string, string>? Messages { get; }
    }
    public class TestSuccessEvent : ISuccessEvent
    {
        public TestSuccessEvent(Guid id, long version)
        {
            Id = id;
            Version = version;
        }

        public Guid Id { get; }
        public long Version { get; }
    }
    public class TesteEntity : Aggregate
    {
        public TesteEntity(IEnumerable<IDomainEvent> persistedEvents) : base(persistedEvents)
        {
        }

        public void SetEntityInvalid()
        {
            ValidationResult = new ValidationResult(new ValidationFailure[] {new ValidationFailure("teste", "teste message")});
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