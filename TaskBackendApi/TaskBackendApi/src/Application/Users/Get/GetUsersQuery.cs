using Application.Abstractions.Messaging;
using Application.Users.GetByEmail;

namespace Application.Users.Get;

public sealed record GetUsersQuery(): IQuery<List<UserResponse>>;
