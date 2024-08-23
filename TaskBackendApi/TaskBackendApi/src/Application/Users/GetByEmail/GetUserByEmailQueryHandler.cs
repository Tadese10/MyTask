using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(IUserRepository context)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        User? data = await context.FindOneAsync(u => u.Email == query.Email, cancellationToken);

        if (data is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        }

        var user = new UserResponse
        {
            Id = data.Id,
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email
        };

        return user;
    }
}
