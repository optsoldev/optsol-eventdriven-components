using MediatR;

namespace EventDriven.Arch.Application.Commands;

public interface ICommand : IRequest<Unit>
{
    public Guid IntegrationId { get; init; }
}

public interface IQuery<out TQueryResponse> : IRequest<TQueryResponse> where TQueryResponse : IQueryResponse{}

public interface IQueryResponse
{
    
}