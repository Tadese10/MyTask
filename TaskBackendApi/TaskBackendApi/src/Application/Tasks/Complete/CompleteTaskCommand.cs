using Application.Abstractions.Messaging;

namespace Application.Taskss.Complete;

public sealed record CompleteTaskCommand(Guid TasksItemId) : ICommand;
