using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.List.Get;
using Application.Taskss.Get;
using Domain.List;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.List.Tasks;

internal sealed class GetListTasksQueryHandler(ITasksRepository tasksRepository) : IQueryHandler<GetListTaskQuery, object>
{
    public async Task<Result<object>> Handle(GetListTaskQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<TaskItem> data = await tasksRepository.FindAsync(d=>d.ListId == query.ListItemId, cancellationToken);

        var items = data.Select(TasksItem => new ListTaskResponse
        {
            Id = TasksItem.Id,
            UserId = TasksItem.UserId,
            Description = TasksItem.Description,
            EndDate = TasksItem.EndDate,
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
