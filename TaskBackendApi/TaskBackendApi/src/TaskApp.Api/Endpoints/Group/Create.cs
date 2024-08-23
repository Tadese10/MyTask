using Application.Group.Create;
using Application.List.Create;
using Application.Taskss.Create;
using Domain.Group;
using Domain.List;
using Domain.Tasks;
using MediatR;
using SharedKernel;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace TaskApp.Api.Endpoints.Groups;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public int GroupType { get; set; }
        public string Name { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Groups", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateGroupCommand
            {
                GroupType = (GroupType)request.GroupType,
                Name = request.Name
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Groups)
        .RequireAuthorization();
    }
}
