using Application.Abstractions.Messaging;

namespace Application.Taskss.Get;

public sealed record GetTasksQuery(Guid UserId): IQuery<List<TaskResponse>>;
