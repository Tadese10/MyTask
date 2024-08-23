using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Tasks;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Application.Taskss.Update;

internal sealed class UpdateTaskCommandHandler(IUserRepository userRepository, ITasksRepository TasksRepository)
    : ICommandHandler<UpdateTaskCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        bool validateAllAssignedUsers = await userRepository.ExistsAsync(u => command.AssignedUsers.Contains(u.Id), cancellationToken);

        if (!validateAllAssignedUsers)
        {
            return Result.Failure<Guid>(UserErrors.InvalidAssignedUsers);
        }


        TaskItem? taskData = await TasksRepository.FindOneAsync(u => u.Id == command.Id, cancellationToken);

        if (taskData == null)
        {
            return Result.Failure<Guid>(TaskItemErrors.NotFound(command.Id));
        }

        taskData.Description = command.Description;
        taskData.Priority = command.Priority;
        taskData.StartDate = command.StartDate;
        taskData.EndDate = command.EndDate;
        taskData.ListId = command.ListId;
        taskData.GroupId = command.GroupId;
        taskData.AssignedUsers = command.AssignedUsers;
        taskData.Title = command.Title;

        taskData.Raise(new TaskItemCreatedDomainEvent(taskData.Id));

        await TasksRepository.UpdateAsync(taskData, cancellationToken);

        return taskData.Id;
    }
}
