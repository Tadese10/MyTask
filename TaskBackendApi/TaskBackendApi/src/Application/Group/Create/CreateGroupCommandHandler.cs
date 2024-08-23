using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Group.Create;
using Domain.Common;
using Domain.Group;
using Domain.List;
using Domain.Tasks;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Group.Create;

internal sealed class CreateGroupCommandHandler(IGroupRepository listRepository, IHttpContextAccessor accessor)
    : ICommandHandler<CreateGroupCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
    {
        var data = new GroupItem
        {
            CreatedByUserId = Guid.Parse(accessor.HttpContext.Items[Constants.UserIDKey]?.ToString() ?? Guid.Empty.ToString()),
            Name = command.Name,
            GroupType = command.GroupType,
        };

        data.Raise(new TaskItemCreatedDomainEvent(data.Id));

        await listRepository.AddAsync(data, cancellationToken);

        return data.Id;
    }
}
