using MassTransit;
using Optsol.EventDriven.Components.MassTransit.Acessors;

namespace Optsol.EventDriven.Components.MassTransit;

public abstract class BaseConsumer<T> : IConsumer<T> where T : class
{
    private readonly IConsumeContextAccessor consumeContextAccessor;

    protected BaseConsumer(IConsumeContextAccessor consumeContextAccessor)
    {
        this.consumeContextAccessor = consumeContextAccessor;
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        consumeContextAccessor.SetContext(context);
        await Handle(context);
    }

    protected abstract Task Handle(ConsumeContext<T> context);
}