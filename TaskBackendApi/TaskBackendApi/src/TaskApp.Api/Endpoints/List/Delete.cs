using Application.List.Delete;
using Application.Taskss.Delete;
using MediatR;
using SharedKernel;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace TaskApp.Api.Endpoints.Lists;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("Lists/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteListCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Lists)
        .RequireAuthorization();
    }
}
