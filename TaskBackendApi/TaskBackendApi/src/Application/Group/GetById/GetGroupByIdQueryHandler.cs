using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Group.Get;
using Application.List.Get;
using Domain.Group;
using Domain.List;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Group.GetById;

internal sealed class GetGroupByIdQueryHandler(IGroupRepository groupRepository) : IQueryHandler<GetGroupByIdQuery, GroupResponse>
{
    public async Task<Result<GroupResponse>> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken)
    {
        GroupItem item = await groupRepository.FindOneAsync(item => item.Id == query.GroupItemId, cancellationToken);
        if (item is null)
        {
            return Result.Failure<GroupResponse>(Domain.List.ListItemErrors.NotFound(query.GroupItemId));
        }

        var Tasks = new GroupResponse
        {
            Id = item.Id,
            UserId = item.CreatedByUserId,
            Name = item.Name,
            CreatedAt = item.CreatedOn,
            GroupType = item.GroupType
        };


        return Tasks;
    }
}
