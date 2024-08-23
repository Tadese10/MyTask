using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.List;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SharedKernel;

namespace Application.List.Complete;

internal sealed class CompleteListCommandHandler(IListRepository listRepository, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CompleteListCommand>
{
    public async Task<Result> Handle(CompleteListCommand command, CancellationToken cancellationToken)
    {
        ListItem? TasksItem =  await listRepository.FindOneAsync(t => t.Id == command.ListItemId, cancellationToken);

        if (TasksItem == null)
        {
            return Result.Failure(ListItemErrors.NotFound(command.ListItemId));
        }

        // Tasks: What if it's already completed? Throw an exception? Return a failure?
        TasksItem.IsCompleted = true;
        TasksItem.CompletedAt = dateTimeProvider.UtcNow;

        TasksItem.Raise(new ListItemCompletedDomainEvent(TasksItem.Id));

        await listRepository.UpdateAsync(TasksItem, cancellationToken);

        return Result.Success();
    }
}
