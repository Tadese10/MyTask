using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SharedKernel;

namespace Application.Taskss.Delete;

internal sealed class DeleteTasksCommandHandler(ITasksRepository tasksRepository)
    : ICommandHandler<DeleteTaskCommand>
{
    public async Task<Result> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        TaskItem TasksItem = await tasksRepository.FindOneAsync(t => t.Id ==  command.TasksItemId, cancellationToken);

        if (TasksItem == null)
        {
            return Result.Failure(TaskItemErrors.NotFound(command.TasksItemId));
        }

        await tasksRepository.DeleteByIdAsync(TasksItem.Id, cancellationToken);

        TasksItem.Raise(new TaskItemDeletedDomainEvent(TasksItem.Id));

        return Result.Success();
    }
}
