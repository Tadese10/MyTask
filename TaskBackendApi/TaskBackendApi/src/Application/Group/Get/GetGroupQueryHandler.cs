using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Group.Get;
using Application.List.Get;
using Domain.Group;
using Domain.List;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.List.Get;

internal sealed class GetGroupQueryHandler(IGroupRepository groupRepository) : IQueryHandler<GetGroupQuery, List<GroupResponse>>
{
    public async Task<Result<List<GroupResponse>>> Handle(GetGroupQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<GroupItem> item = await groupRepository.GetAllAsync(cancellationToken);
        var data = item.Select(d => new GroupResponse
        {
            Id = d.Id,
            UserId = d.CreatedByUserId,
            Name = d.Name,
            CreatedAt = d.CreatedOn,
            GroupType = d.GroupType
        }).ToList();

        return Result.Success(data);
    }
}
