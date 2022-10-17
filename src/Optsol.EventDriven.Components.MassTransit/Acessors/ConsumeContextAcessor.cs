using MassTransit;

namespace Optsol.EventDriven.Components.MassTransit.Acessors;

public class ConsumeContextAcessor : IConsumeContextAccessor
{
    public void SetContext(ConsumeContext? context)
    {
        Context = context;
    }

    public ConsumeContext? Context { get; private set; }
    
}