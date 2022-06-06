using Newtonsoft.Json;

namespace Optsol.EventDriven.Components.Core.Domain.Entities
{

    public class DomainEventConverter : IDomainEventConverter
    {
        private readonly IDomainEventRegister _register;

        public DomainEventConverter(IDomainEventRegister register)
        {
            _register = register;
        }

        public IDomainEvent Convert(string eventData)
        {

            Type eventType = _register.Get(eventData);
            if (eventType is null)
                throw new InvalidCastException("Nao Existe Evento registrado.");

            var @event = (IDomainEvent)JsonConvert.DeserializeObject(eventData, eventType);
            return @event;
        }
    }
}
