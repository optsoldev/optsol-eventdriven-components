using MediatR;

namespace Optsol.EventDriven.Components.Core.Application;

public interface IQuery<out TQueryResponse> : IRequest<TQueryResponse>{ }