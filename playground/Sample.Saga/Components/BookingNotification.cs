namespace Sample.Saga.Components;

public record BookingNotification
{
    public Guid CorrelationId { get; set; }
} 
