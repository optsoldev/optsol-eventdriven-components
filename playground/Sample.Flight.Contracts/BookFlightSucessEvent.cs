using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public class BookFlightSucessEvent : SuccessEvent
{
  public BookFlightSucessEvent(Guid Id, long Version) : base(Id, Version)
  {}
}