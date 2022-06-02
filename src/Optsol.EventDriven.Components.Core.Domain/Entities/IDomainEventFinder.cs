namespace Optsol.EventDriven.Components.Core.Domain.Entities
{
    public interface IDomainEventFinder
    {
        Type Get(string eventData);
    }
}
