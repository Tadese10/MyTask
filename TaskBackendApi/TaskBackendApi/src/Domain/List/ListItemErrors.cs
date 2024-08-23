using SharedKernel;

namespace Domain.List;

public static class ListItemErrors
{
    public static Error NotFound(Guid ListItemId) => Error.NotFound(
        "ListItems.NotFound",
        $"The List item with the Id = '{ListItemId}' was not found");
}
