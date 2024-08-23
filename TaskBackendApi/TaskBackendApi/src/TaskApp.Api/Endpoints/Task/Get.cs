using Application.Taskss.Get;
using MediatR;
using SharedKernel;
using TaskApp.Api.Endpoints;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Tasks", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetTasksQuery(userId);

            Result<List<TaskResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .RequireAuthorization();
    }
}
