using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.List.Get;
using Application.Users.GetByEmail;
using Domain.List;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Get;

internal sealed class GetUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<User> data = await userRepository.GetAllAsync(cancellationToken);
        var items = data.Select(item => new UserResponse
        {
            Id = item.Id,
            Email = item.Email,
            FirstName = item.FirstName,
            LastName = item.LastName,
        }).ToList();

        return Result.Success(items);
    }
}
