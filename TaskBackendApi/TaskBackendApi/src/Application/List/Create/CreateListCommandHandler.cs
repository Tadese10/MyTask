using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.List;
using Domain.Tasks;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.List.Create;

internal sealed class CreateListCommandHandler(IListRepository listRepository, IHttpContextAccessor accessor)
    : ICommandHandler<CreateListCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateListCommand command, CancellationToken cancellationToken)
    {
        var ListItem = new ListItem
        {
            CreatedByUserId = Guid.Parse(accessor.HttpContext.Items[Constants.UserIDKey]?.ToString() ?? Guid.Empty.ToString()),
            Description = command.Description,
        };

        ListItem.Raise(new TaskItemCreatedDomainEvent(ListItem.Id));

        await listRepository.AddAsync(ListItem, cancellationToken);

        return ListItem.Id;
    }
}
