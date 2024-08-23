using Application.Abstractions.Messaging;

namespace Application.Taskss.GetById;

public sealed record GetTaskByIdQuery(Guid TaskItemId) : IQuery<TasksResponse>;
