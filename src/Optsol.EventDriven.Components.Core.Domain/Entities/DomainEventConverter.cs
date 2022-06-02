using Newtonsoft.Json;

namespace Optsol.EventDriven.Components.Core.Domain.Entities
{

    public class DomainEventConverter : IDomainEventConverter
    {
        private readonly IDomainEventFinder domainEventFinder;

        public DomainEventConverter(IDomainEventFinder domainEventFinder)
        {
            this.domainEventFinder = domainEventFinder;
        }

        public IDomainEvent Convert(string eventData)
        {

            Type eventType = domainEventFinder.Get(eventData);
            if (eventType is null)
                throw new InvalidCastException("Nao Existe Evento registrado.");

            var @event = (IDomainEvent)JsonConvert.DeserializeObject(eventData, eventType);
            return @event;
        }
    }
}
