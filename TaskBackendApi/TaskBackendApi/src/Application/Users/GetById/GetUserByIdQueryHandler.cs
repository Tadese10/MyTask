using System.Text.Json.Serialization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(IUserRepository context, ICacheService cacheService)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        UserResponse responseData = null;
        responseData = await cacheService.GetAsync<UserResponse>(query.UserId.ToString(), cancellationToken);
        if(responseData == null)
        {
            User? _data = await context.FindOneAsync(u => u.Id == query.UserId, cancellationToken);

            if (_data is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
            }

             responseData = new UserResponse
            {
                Id = _data!.Id,
                FirstName = _data.FirstName,
                LastName = _data.LastName,
                Email = _data.Email
            };

            await cacheService.SetAsync(query.UserId.ToString(), responseData, cancellationToken: cancellationToken);
        }


        return responseData;
    }
}
