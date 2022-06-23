using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;


namespace Optsol.EventDriven.Components.Core.Domain.Entities
{
    public class DomainEventRegister : IDomainEventRegister
    {
        public record RegisteredDomainEvent(Type domainEvent, JSchema schema);

        public readonly Dictionary<string, RegisteredDomainEvent> schemas = new();

        public void Register(Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(IDomainEvent))) throw new InvalidCastException("Tipo deve implementar IEvent");

            AddJsonSchema(type);
        }

        private void AddJsonSchema(Type type)
        {
            JSchemaGenerator generator = new JSchemaGenerator();

            JSchema schemaGen = generator.Generate(type);

            schemas.Add(type.ToString(), new RegisteredDomainEvent(type, schemaGen));
        }
        public Type Get(string eventData)
        {
            return schemas.FirstOrDefault(x => JObject.Parse(eventData).IsValid(x.Value.schema)).Value.domainEvent;
        }
    }
}
