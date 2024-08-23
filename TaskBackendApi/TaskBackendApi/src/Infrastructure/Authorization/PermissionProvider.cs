namespace Infrastructure.Authorization;

internal sealed class PermissionProvider
{
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public Task<HashSet<string>> GetForUserIdAsync(Guid userId)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        // Tasks: Here you'll implement your logic to fetch permissions.
        HashSet<string> permissionsSet = [userId.ToString()];

        return Task.FromResult(permissionsSet);
    }
}
