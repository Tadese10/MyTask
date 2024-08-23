using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Group;
using Domain.List;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SharedKernel;

namespace Application.Group.Complete;

internal sealed class CompleteGroupCommandHandler(IGroupRepository listRepository)
    : ICommandHandler<CompleteGroupCommand>
{
    public async Task<Result> Handle(CompleteGroupCommand command, CancellationToken cancellationToken)
    {
        GroupItem? data =  await listRepository.FindOneAsync(t => t.Id == command.GroupItemId, cancellationToken);

        if (data == null)
        {
            return Result.Failure(ListItemErrors.NotFound(command.GroupItemId));
        }

        //// Tasks: What if it's already completed? Throw an exception? Return a failure?
        //data.IsCompleted = true;
        //data.CompletedAt = dateTimeProvider.UtcNow;

        data.Raise(new ListItemCompletedDomainEvent(data.Id));

        await listRepository.UpdateAsync(data, cancellationToken);

        return Result.Success();
    }
}
