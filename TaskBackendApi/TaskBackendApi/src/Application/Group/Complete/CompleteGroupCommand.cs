using Application.Abstractions.Messaging;

namespace Application.Group.Complete;

public sealed record CompleteGroupCommand(Guid GroupItemId) : ICommand;
