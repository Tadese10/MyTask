using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SharedKernel;

namespace Application.Taskss.Complete;

internal sealed class CompleteTaskCommandHandler(ITasksRepository tasksRepository, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CompleteTaskCommand>
{
    public async Task<Result> Handle(CompleteTaskCommand command, CancellationToken cancellationToken)
    {
        TaskItem? TasksItem =  await tasksRepository.FindOneAsync(t => t.Id == command.TasksItemId, cancellationToken);

        if (TasksItem == null)
        {
            return Result.Failure(TaskItemErrors.NotFound(command.TasksItemId));
        }

        // Tasks: What if it's already completed? Throw an exception? Return a failure?
        TasksItem.IsCompleted = true;
        TasksItem.CompletedAt = dateTimeProvider.UtcNow;

        TasksItem.Raise(new TaskItemCompletedDomainEvent(TasksItem.Id));

        await tasksRepository.UpdateAsync(TasksItem, cancellationToken);

        return Result.Success();
    }
}
