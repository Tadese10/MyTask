using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.FindOneAsync(u => u.Email == command.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        }

        bool verified = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!verified)
        {
            return Result.Failure<UserResponse>(UserErrors.IncorrectPassword);
        }

        string token = tokenProvider.Create(user);

        var data = new UserResponse
        {
            Token = token,
            Email = user.Email,
            FirstName = user.FirstName,
            Id = user.Id,
            LastName = user.LastName,
        };

        return data;
    }
}
