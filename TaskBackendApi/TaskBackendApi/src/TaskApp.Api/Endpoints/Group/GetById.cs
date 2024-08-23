using Application.Group.Get;
using Application.Group.GetById;
using Application.List.GetById;
using Application.Taskss.GetById;
using MediatR;
using SharedKernel;
using TaskApp.Api.Endpoints;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace Web.Api.Endpoints.Groups;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Groups/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetGroupByIdQuery(id);

            Result<GroupResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Groups)
        .RequireAuthorization();
    }
}
