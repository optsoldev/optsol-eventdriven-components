using MassTransit;
using Sample.Flight.Contracts;

namespace Sample.Flight.Driving.Commands.Consumers;


public class RequestClientSampleQueryConsumer : IConsumer<RequestClientSampleQuery>
{
    public Task Consume(ConsumeContext<RequestClientSampleQuery> context)
    {
        return context.RespondAsync(new RequestClientSampleResult());
    }
}