using Application.Abstractions.Messaging;

namespace Application.List.Delete;

public sealed record DeleteListCommand(Guid ListItemId) : ICommand;
