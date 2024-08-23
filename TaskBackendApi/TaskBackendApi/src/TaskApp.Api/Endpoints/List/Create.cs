using Application.List.Create;
using Application.Taskss.Create;
using Domain.List;
using Domain.Tasks;
using MediatR;
using SharedKernel;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace TaskApp.Api.Endpoints.Lists;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public int ListType { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Lists", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateListCommand
            {
                UserId = request.UserId,
                Description = request.Description,
                ListType = (ListType)request.ListType
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Lists)
        .RequireAuthorization();
    }
}
