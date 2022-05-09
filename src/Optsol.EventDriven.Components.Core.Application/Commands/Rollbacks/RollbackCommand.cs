using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Commands.Rollbacks;

public record RollbackCommand(Guid IntegrationId) : ICommand;