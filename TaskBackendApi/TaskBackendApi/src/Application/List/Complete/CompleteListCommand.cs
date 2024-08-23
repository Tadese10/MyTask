using Application.Abstractions.Messaging;

namespace Application.List.Complete;

public sealed record CompleteListCommand(Guid ListItemId) : ICommand;
