namespace Optsol.EventDriven.Components.Core.Contracts;

public interface ISagaContract
{
   public Guid CorrelationId { get; }
}