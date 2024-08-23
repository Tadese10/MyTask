using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.List.Get;
using Domain.List;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.List.GetById;

internal sealed class GetListByIdQueryHandler(IListRepository listRepository) : IQueryHandler<GetListByIdQuery, ListResponse>
{
    public async Task<Result<ListResponse>> Handle(GetListByIdQuery query, CancellationToken cancellationToken)
    {
        ListItem task = await listRepository.FindOneAsync(item => item.Id == query.ListItemId, cancellationToken);
        if (task is null)
        {
            return Result.Failure<ListResponse>(Domain.List.ListItemErrors.NotFound(query.ListItemId));
        }

        var Tasks = new ListResponse
        {
            Id = task.Id,
            UserId = task.CreatedByUserId,
            Description = task.Description,
            ListType = task.ListType,
            IsCompleted = task.IsCompleted
        };


        return Tasks;
    }
}
