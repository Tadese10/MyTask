using Application.Taskss.Create;
using Domain.Tasks;
using MediatR;
using SharedKernel;
using TaskApp.Api.Endpoints;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Priority { get; set; }
        public Guid ListId { get; set; }
        public Guid GroupId { get; set; }
        public List<Guid> AssignedUsers { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Tasks", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateTaskCommand
            {
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AssignedUsers = request.AssignedUsers,
                Priority =(Priority)request.Priority,
                GroupId = request.GroupId,
                ListId = request.ListId,
                Title = request.Title
            };

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .RequireAuthorization();
    }
}
