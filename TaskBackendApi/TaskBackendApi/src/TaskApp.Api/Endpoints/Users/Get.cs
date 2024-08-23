using Application.List.Get;
using Application.Taskss.Get;
using Application.Users.Get;
using MediatR;
using SharedKernel;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace TaskApp.Api.Endpoints.Users;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetUsersQuery();

            Result<List<UserResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization();
    }
}
