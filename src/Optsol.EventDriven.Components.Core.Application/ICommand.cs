using MediatR;

namespace Optsol.EventDriven.Components.Core.Application;

public interface ICommand : IRequest<Unit>
{
}