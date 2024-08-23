using Application.Taskss.Delete;
using MediatR;
using SharedKernel;
using TaskApp.Api.Endpoints;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("Tasks/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteTaskCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .RequireAuthorization();
    }
}
