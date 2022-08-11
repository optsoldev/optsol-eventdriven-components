using FluentAssertions;
using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification.Unit.Tests
{
    public class MessageBusSpec
    {
        [Fact]
        public void ShouldSerializeEventCorrectly()
        {
            var evento = new Evento();

            var eventoSerializado = JsonConvert.SerializeObject(evento, evento.GetType(), Formatting.None, new JsonSerializerSettings {  });

            eventoSerializado.Should().Contain("Teste");
        }

        [JsonObject]
        public class Evento : IDomainEvent
        {
            public Guid ModelId => Guid.NewGuid();

            public long ModelVersion => 1;

            public DateTime When => DateTime.UtcNow;

            public string Teste = "Teste";
        }
    }
}