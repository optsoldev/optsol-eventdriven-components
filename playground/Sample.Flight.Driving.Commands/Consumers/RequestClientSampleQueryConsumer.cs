using MassTransit;
using Optsol.EventDriven.Components.Core.Contracts;
using Sample.Flight.Contracts;

namespace Sample.Flight.Driving.Commands.Consumers;

public interface IRequestClientSampleQueryTesteConsumerAddress : IConsumerAddress {}

public class RequestClientSampleQueryConsumer : IConsumer<RequestClientSampleQuery>,
    IRequestClientSampleQueryTesteConsumerAddress
{
    public Task Consume(ConsumeContext<RequestClientSampleQuery> context)
    {
        return context.RespondAsync(new RequestClientSampleResult());
    }
}