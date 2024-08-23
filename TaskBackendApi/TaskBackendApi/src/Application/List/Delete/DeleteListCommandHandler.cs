using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.List;
using Domain.Tasks;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SharedKernel;

namespace Application.List.Delete;

internal sealed class DeleteListCommandHandler(IListRepository listRepository)
    : ICommandHandler<DeleteListCommand>
{
    public async Task<Result> Handle(DeleteListCommand command, CancellationToken cancellationToken)
    {
        ListItem listItem = await listRepository.FindOneAsync(t => t.Id ==  command.ListItemId, cancellationToken);

        if (listItem == null)
        {
            return Result.Failure(ListItemErrors.NotFound(command.ListItemId));
        }

        await listRepository.DeleteAsync(d=> d.Id == listItem.Id, cancellationToken);

        listItem.Raise(new TaskItemDeletedDomainEvent(listItem.Id));

        return Result.Success();
    }
}
