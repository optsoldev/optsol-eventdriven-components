namespace Optsol.EventDriven.Components.Core.Domain.Entities
{
    public interface IDomainEventConverter
    {
        public IDomainEvent Convert(string eventData);
    }
}
