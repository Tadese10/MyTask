using Application.Abstractions.Messaging;

namespace Application.Group.Delete;

public sealed record DeleteGroupCommand(Guid GroupItemId) : ICommand;
