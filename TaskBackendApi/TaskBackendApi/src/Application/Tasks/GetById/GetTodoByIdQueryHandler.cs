using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Taskss.GetById;

internal sealed class GetTasksByIdQueryHandler(ITasksRepository tasksRepository) : IQueryHandler<GetTaskByIdQuery, TasksResponse>
{
    public async Task<Result<TasksResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        TaskItem task = await tasksRepository.FindOneAsync(TaskItem => TaskItem.Id == query.TaskItemId, cancellationToken);
        if (task is null)
        {
            return Result.Failure<TasksResponse>(TaskItemErrors.NotFound(query.TaskItemId));
        }

        var Tasks = new TasksResponse
        {
            Id = task.Id,
            UserId = task.UserId,
            Description = task.Description,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Title = task.Title,
            AssignedUsers = task.AssignedUsers,
            GroupId = task.GroupId,
            ListId = task.ListId,
            Priority = task.Priority,
        };


        return Tasks;
    }
}
