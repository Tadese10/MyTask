using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Tasks;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Application.Taskss.Create;

internal sealed class CreateTasksCommandHandler(IUserRepository userRepository, IHttpContextAccessor accessor, ITasksRepository TasksRepository, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateTaskCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        bool validateAllAssignedUsers = await userRepository.ExistsAsync(u => command.AssignedUsers.Contains(u.Id), cancellationToken);

        if (!validateAllAssignedUsers)
        {
            return Result.Failure<Guid>(UserErrors.InvalidAssignedUsers);
        }

        var TasksItem = new TaskItem
        {
            UserId = Guid.Parse(accessor.HttpContext.Items[Constants.UserIDKey]?.ToString() ?? Guid.Empty.ToString()),
            Description = command.Description,
            Priority = command.Priority,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            ListId = command.ListId,
            GroupId = command.GroupId,
            IsCompleted = false,
            CreatedAt = dateTimeProvider.UtcNow,
            Status = Status.InReview,
            AssignedUsers = command.AssignedUsers,
            Title = command.Title,
        };

        TasksItem.Raise(new TaskItemCreatedDomainEvent(TasksItem.Id));

        await TasksRepository.AddAsync(TasksItem, cancellationToken);

        return TasksItem.Id;
    }
}
