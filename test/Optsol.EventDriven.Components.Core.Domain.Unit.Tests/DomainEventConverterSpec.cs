﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Optsol.EventDriven.Components.Core.Domain.Unit.Tests
{
    public class DomainEventConverterSpec
    {
        [Fact]
        public void Should_Convert_StringData_To_IEvent()
        {
            var register = new DomainEventRegister();
            register.Register(typeof(EventoTeste));
            register.Register(typeof(EventoTeste2));

            var evt = new EventoTeste2(Guid.NewGuid(), Guid.NewGuid(), 1, DateTime.UtcNow, DateTime.Now);

            var converter = new DomainEventConverter(register);

            var result = converter.Convert(JsonSerializer.Serialize(evt));

            result.Should().BeOfType(typeof(EventoTeste2));

            result.As<EventoTeste2>().AnotherProperty.Should().Be(evt.AnotherProperty);
        }
    }
}