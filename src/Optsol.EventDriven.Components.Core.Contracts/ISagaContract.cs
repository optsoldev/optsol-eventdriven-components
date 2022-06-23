namespace Sample.Saga.Contracts;

public interface ISagaContract
{
    Guid CorrelationId { get; }
}