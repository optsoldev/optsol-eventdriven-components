using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Commands.Commits;

public record CommitCommand(Guid IntegrationId) : ICommand;