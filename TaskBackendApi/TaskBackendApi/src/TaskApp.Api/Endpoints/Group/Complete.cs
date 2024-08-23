using Application.List.Complete;
using Application.Taskss.Complete;
using Application.Taskss.Delete;
using MediatR;
using SharedKernel;

//namespace Web.Api.Endpoints.Lists;

//internal sealed class Complete : IEndpoint
//{
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapPut("Lists/{id:guid}/complete", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
//        {
//            var command = new CompleteListCommand(id);

//            Result result = await sender.Send(command, cancellationToken);

//            return result.Match(Results.NoContent, CustomResults.Problem);
//        })
//        .WithTags(Tags.Lists)
//        .RequireAuthorization();
//    }
//}
