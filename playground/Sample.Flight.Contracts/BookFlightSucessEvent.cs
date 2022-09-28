using Optsol.EventDriven.Components.Core.Domain.Entities.Events;

namespace Sample.Flight.Contracts;

public class BookFlightSucessEvent : SuccessEvent
{
  public BookFlightSucessEvent(Guid id, long  version, Guid userId) : base(id, version, userId)
  {}
}