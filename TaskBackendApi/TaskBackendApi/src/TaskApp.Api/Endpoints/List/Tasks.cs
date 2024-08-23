﻿using Application.List.Get;
using Application.List.Tasks;
using Application.Taskss.Get;
using MediatR;
using SharedKernel;
using TaskApp.Api.Extensions;
using TaskApp.Api.Infrastructure;

namespace TaskApp.Api.Endpoints.Lists;

internal sealed class Tasks : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Lists/{id:guid}/Tasks", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetListTaskQuery(id);

            Result<object> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Lists)
        .RequireAuthorization();
    }
}
