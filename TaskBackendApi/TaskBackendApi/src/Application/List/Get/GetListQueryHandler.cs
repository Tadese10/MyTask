using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.List.Get;
using Domain.List;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.List.Get;

internal sealed class GetListQueryHandler(IListRepository listRepository, ITasksRepository tasksRepository) : IQueryHandler<GetListQuery, object>
{
    public async Task<Result<object>> Handle(GetListQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<ListItem> data = await listRepository.GetAllAsync(cancellationToken);

        var items = data.Select( TasksItem => new ListResponse
        {
            Id = TasksItem.Id,
            UserId = TasksItem.CreatedByUserId,
            Description = TasksItem.Description,
            ListType = TasksItem.ListType,
            CompletedAt = TasksItem.CompletedAt,
            Count = Task.Run(() => GetCount(TasksItem.Id)).Result 
    }).Distinct().ToList();

        return Result.Success(items);
    }

    private async Task<int> GetCount(Guid Id)
    {
        IReadOnlyList<TaskItem> data = await tasksRepository.FindAsync(d => d.ListId == Id);
        return data.Count;
    }
}
