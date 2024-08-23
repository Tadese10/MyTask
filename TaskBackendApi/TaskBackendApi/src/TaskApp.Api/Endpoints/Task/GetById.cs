using Application.Taskss.GetById;
using MediatR;
using SharedKernel;
using TaskApp.Api.Endpoints;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Taskss/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetTaskByIdQuery(id);

            Result<TasksResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .RequireAuthorization();
    }
}
