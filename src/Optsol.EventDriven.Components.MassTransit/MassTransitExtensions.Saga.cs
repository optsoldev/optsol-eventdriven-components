using MassTransit;
using MassTransit.SagaStateMachine;

namespace Optsol.EventDriven.Components.MassTransit;

public partial class MassTransitExtensions
{
    /// <summary>
    /// An agnostic way to name the destination address using the Command name as the base.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="messageFactory"></param>
    /// <param name="exchangeType"></param>
    /// <typeparam name="TSaga"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    public static EventActivityBinder<TSaga, TData> ExecuteAsync<TSaga, TData, TMessage>(this EventActivityBinder<TSaga, TData> source,
        Func<BehaviorContext<TSaga, TData>, Task<SendTuple<TMessage>>> messageFactory,
        ExchangeType exchangeType = ExchangeType.Queue)
        where TSaga : class, SagaStateMachineInstance
        where TData : class
        where TMessage : class
    {
        var destinationAddress = MessageBusUri.GetInstance().CreateUri(typeof(TMessage), exchangeType);
        
        return source.Add(new SendActivity<TSaga, TData, TMessage>(_ => destinationAddress, MessageFactory<TMessage>.Create(messageFactory)));
    }

    /// <summary>
    /// An agnostic way (rabbitmq or azure service bus) to name the destination address passing the destination address string.
    /// <example>DestinationAddress - BookFlightCommand - returns prefix-book-flight</example>
    /// <example>DestinationAddress - BookFligh - returns prefix-book-flight</example>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destinationAddress">CamelCase route.</param>
    /// <param name="messageFactory"></param>
    /// <param name="exchangeType"></param>
    /// <typeparam name="TSaga"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    public static EventActivityBinder<TSaga, TData> ExecuteAsync<TSaga, TData, TMessage>(this EventActivityBinder<TSaga, TData> source,
        string destinationAddress,
        Func<BehaviorContext<TSaga, TData>, Task<SendTuple<TMessage>>> messageFactory,
        ExchangeType exchangeType = ExchangeType.Queue)
        where TSaga : class, SagaStateMachineInstance
        where TData : class
        where TMessage : class
    {
        var uri = MessageBusUri.GetInstance().CreateUri(destinationAddress, exchangeType);
        
        return source.Add(new SendActivity<TSaga, TData, TMessage>(_ => uri, MessageFactory<TMessage>.Create(messageFactory)));
    }
}