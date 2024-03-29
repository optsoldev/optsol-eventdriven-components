namespace Optsol.EventDriven.Components.Core.Domain;

public interface INotificator
{
    Task Publish<T>(T @event);

    Task Publish<T>(params T[] @events);
}