using FluentAssertions;
using Optsol.EventDriven.Components.Core.Domain.Entities;

namespace Optsol.EventDriven.Components.Core.Domain.Unit.Tests
{
    public class DomainEventRegisterSpec
    {
        
    }
    public record FakeEvent(Guid IntegrationId) { }
    public record EventoTeste(Guid TransactionId, Guid Id, long Version, DateTime When): IDomainEvent { }
    public record EventoTeste2(Guid TransactionId, Guid Id, long Version, DateTime When, DateTime AnotherProperty) : IDomainEvent { }
}
