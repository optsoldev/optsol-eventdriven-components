namespace Sample.Saga.Contracts;

public interface ISaga
{
    Guid CorrelationId { get; }
}