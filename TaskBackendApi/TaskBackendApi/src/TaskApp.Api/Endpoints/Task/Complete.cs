using Application.Taskss.Complete;
using Application.Taskss.Delete;
using MediatR;
using SharedKernel;

//namespace Web.Api.Endpoints.Task;

//internal sealed class Complete : IEndpoint
//{
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapPut("Tasks/{id:guid}/complete", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
//        {
//            var command = new CompleteTaskCommand(id);

//            Result result = await sender.Send(command, cancellationToken);

//            return result.Match(Results.NoContent, CustomResults.Problem);
//        })
//        .WithTags(Tags.Tasks)
//        .RequireAuthorization();
//    }
//}
