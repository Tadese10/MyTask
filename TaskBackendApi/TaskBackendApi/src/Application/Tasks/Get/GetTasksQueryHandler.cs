using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Taskss.Get;

internal sealed class GetTasksQueryHandler(ITasksRepository tasksRepository) : IQueryHandler<GetTasksQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetTasksQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Tasks.TaskItem> data = await tasksRepository.FindAsync(TasksItem => TasksItem.UserId == query.UserId, cancellationToken);
        var items = data.Select(TasksItem => new TaskResponse
        {
            Id = TasksItem.Id,
            UserId = TasksItem.UserId,
            Description = TasksItem.Description,
            EndDate= TasksItem.EndDate,
            Title = TasksItem.Title,
            Status = TasksItem.Status,
            CreatedAt = TasksItem.CreatedAt,
            CompletedAt = TasksItem.CompletedAt,
            AssignedUsers = TasksItem.AssignedUsers,
            GroupId = TasksItem.GroupId,
            ListId = TasksItem.ListId,
            Priority = TasksItem.Priority,
            StartDate = TasksItem.StartDate,
        }).ToList();

        return Result.Success(items);
    }
}
