namespace Optsol.EventDriven.Components.Core.Application.Commands.Commits;

public record CommitCommand(Guid IntegrationId) : ICommand;