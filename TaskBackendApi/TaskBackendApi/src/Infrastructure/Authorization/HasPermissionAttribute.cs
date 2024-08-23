using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

#pragma warning disable S3993 // Custom attributes should be marked with "System.AttributeUsageAttribute"
public sealed class HasPermissionAttribute : AuthorizeAttribute
#pragma warning restore S3993 // Custom attributes should be marked with "System.AttributeUsageAttribute"
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
