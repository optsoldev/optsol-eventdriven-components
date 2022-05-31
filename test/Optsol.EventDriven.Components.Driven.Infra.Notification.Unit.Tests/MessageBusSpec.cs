using FluentAssertions;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification.Unit.Tests
{
    public class MessageBusSpec
    {
        [Fact]
        public void ShouldSerializeEventCorrectly()
        {
            var evento = new Evento();

            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                
            };

            var eventoSerializado = JsonSerializer.Serialize(evento, evento.GetType(), options);

            eventoSerializado.Should().Contain("Teste");
        }

        [DataContract]
        public class Evento : IDomainEvent
        {
            public Guid ModelId => Guid.NewGuid();

            public int ModelVersion => 1;

            public DateTime When => DateTime.UtcNow;

            public string Teste = "Teste";
        }
    }
}