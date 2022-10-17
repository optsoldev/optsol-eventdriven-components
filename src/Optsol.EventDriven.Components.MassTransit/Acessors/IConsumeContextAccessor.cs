using MassTransit;

namespace Optsol.EventDriven.Components.MassTransit.Acessors;

public interface IConsumeContextAccessor
{
    ConsumeContext? Context { get; }
    void SetContext(ConsumeContext? context);
    
}