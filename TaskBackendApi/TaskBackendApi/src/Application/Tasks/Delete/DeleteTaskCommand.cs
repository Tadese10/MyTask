using Application.Abstractions.Messaging;

namespace Application.Taskss.Delete;

public sealed record DeleteTaskCommand(Guid TasksItemId) : ICommand;
