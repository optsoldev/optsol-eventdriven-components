using FluentAssertions;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Unit.Tests
{
    public class DomainEventRegisterSpec
    {
        [Fact]
        public void Should_Register_IEvent_Types()
        {
            var register = new DomainEventRegister();
            register.Register(typeof(EventoTeste));
            register.Register(typeof(EventoTeste2));


            var events = register.GetTypes();

            events.Should().HaveCount(2);
        }

        [Fact]
        public void Should_Throw_Exception_When_Type_Does_Not_Implement_IEvent()
        {
            var register = new DomainEventRegister();
            Action a = () => register.Register(typeof(FakeEvent));

            a.Should().Throw<InvalidCastException>("Tipo deve implementar IEvent");
        }
    }
    public record FakeEvent(Guid IntegrationId) { }
    public record EventoTeste(Guid IntegrationId, Guid ModelId, int ModelVersion, DateTime When): IDomainEvent { }
    public record EventoTeste2(Guid IntegrationId, Guid ModelId, int ModelVersion, DateTime When, DateTime AnotherProperty) : IDomainEvent { }
}
