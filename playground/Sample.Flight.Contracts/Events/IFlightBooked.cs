﻿using Optsol.EventDriven.Components.Core.Contracts;

namespace Sample.Flight.Contracts.Events;

public record IFlightBooked : ISagaContract
{
    public Guid TravelId { get; set; }

    public Guid CorrelationId { get; set; }
}
