using Optsol.EventDriven.Components.Core.Domain.Entities;


namespace Optsol.EventDriven.Components.Core.Domain
{
    public interface IDomainEventRegister
    {
        public void Register(Type type);

        public IEnumerable<Type> GetTypes();
    }
    public class DomainEventRegister: IDomainEventRegister
    {
        private readonly List<Type> domainEvents = new();

        public IEnumerable<Type> GetTypes()
        {
            return domainEvents;
        }

        public void Register(Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(IEvent))) throw new InvalidCastException("Tipo deve implementar IEvent");
            domainEvents.Add(type);
        }
    }
}
