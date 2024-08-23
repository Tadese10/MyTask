using Application.Group.Get;
using Application.List.Get;
using Application.Taskss.Get;
using MediatR;
using SharedKernel;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace TaskApp.Api.Endpoints.Groups;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Groups", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetGroupQuery();

            Result<List<GroupResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Groups)
        .RequireAuthorization();
    }
}
