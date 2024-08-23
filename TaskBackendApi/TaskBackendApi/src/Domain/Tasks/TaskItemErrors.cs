using SharedKernel;

namespace Domain.Tasks;

public static class TaskItemErrors
{
    public static Error NotFound(Guid TasksItemId) => Error.NotFound(
        "TasksItems.NotFound",
        $"The to-do item with the Id = '{TasksItemId}' was not found");
}
