using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Text.Json;

namespace Optsol.EventDriven.Components.Core.Domain
{
    public interface IDomainEventConverter
    {
        public IEvent Convert(string eventData);
    }

    public class DomainEventConverter : IDomainEventConverter
    {
        private readonly IDomainEventRegister _register;

        public DomainEventConverter(IDomainEventRegister register)
        {
            _register = register;
        }

        public IEvent Convert(string eventData)
        {

            foreach (var type in _register.GetTypes())
            {
                if(TryDeserializeEvent(eventData, type, out var result))
                {
                    return result;
                };
            }

            throw new InvalidCastException("Nao Existe Evento registrado.");
        }

        private bool TryDeserializeEvent(string eventData, Type type, out IEvent result)
        {
            result = default;
            try
            {
                result = JsonConvert.DeserializeObject(eventData, type, new JsonSerializerSettings
                {
                 MissingMemberHandling = MissingMemberHandling.Error
                }) as IEvent;

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
