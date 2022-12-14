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
    public void DeveCriarFailedEvent()
    {
        var entity = new TesteEntity(Enumerable.Empty<IDomainEvent>());
        
        entity.SetEntityInvalid();

        var failedEvent = new FailedEvent(entity.Id, entity.ValidationResult.Errors);

        failedEvent.Should().NotBeNull();
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