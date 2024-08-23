using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Group.Delete;
using Domain.Group;
using Domain.List;
using Domain.Tasks;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SharedKernel;

namespace Application.List.Delete;

internal sealed class DeleteGroupCommandHandler(IGroupRepository groupRepository)
    : ICommandHandler<DeleteGroupCommand>
{
    public async Task<Result> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
    {
        GroupItem data = await groupRepository.FindOneAsync(t => t.Id ==  command.GroupItemId, cancellationToken);

        if (data == null)
        {
            return Result.Failure(GroupItemErrors.NotFound(command.GroupItemId));
        }

        await groupRepository.DeleteAsync(d=> d.Id == data.Id, cancellationToken);

        data.Raise(new TaskItemDeletedDomainEvent(data.Id));

        return Result.Success();
    }
}
