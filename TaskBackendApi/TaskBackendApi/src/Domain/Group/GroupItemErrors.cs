using SharedKernel;

namespace Domain.Group;

public static class GroupItemErrors
{
    public static Error NotFound(Guid ListItemId) => Error.NotFound(
        "ListItems.NotFound",
        $"The List item with the Id = '{ListItemId}' was not found");
}
