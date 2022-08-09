using FluentAssertions;
using Moq;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Core.Domain.Unit.Tests;

public class AggregateSpec
{
    [Fact]
    public void Deve_Converter_SuccessEvents_Para_Tipo_Especifico()
    {
        var entity = new Mock<IAggregate>();

        entity.Setup(s => s.SuccessEvents).Returns(new List<ISuccessEvent>()
        {
            new SuccessEvent(Guid.NewGuid(), 1)
        });

        List<TestSuccessEvent> result = new();
        
        foreach (var successEvent in entity.Object.SuccessEvents)
        {
            var @event = (SuccessEvent) successEvent;
            var eventResult = (TestSuccessEvent)@event;
            
            result.Add(eventResult);
        }
        
        result.First().GetType().Should().Be(typeof(TestSuccessEvent));
    }

    [Fact]
    public void Deve_Converter_SuccessEvent_ParaTipo_Especifico()
    {
        var successEvent = new SuccessEvent(Guid.NewGuid(), 1);

        var result = (TestSuccessEvent) successEvent;

        result.GetType().Should().Be(typeof(TestSuccessEvent));
        
    }

    public record TestSuccessEvent(Guid Id, long Version)
    {
        public static explicit operator TestSuccessEvent(SuccessEvent @event)
        {
            return new TestSuccessEvent(@event.Id, @event.Version);
        }
    }
}