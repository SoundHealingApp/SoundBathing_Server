using Auth.Application.Errors;
using Auth.Application.Interfaces;
using Auth.Core;
using Auth.Core.CQRS;
using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Commands;

public record LoginCommand(string UserName, string Password) : IRequest<Result<string>>;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName);

        if (user == null)
            return new UserNotFoundError(request.UserName);
        
        var result = passwordHasher.Verify(request.Password, user.PasswordHash);
        
        if (!result)
            return new IncorrectPassword();

        var token = jwtProvider.GenerateToken(user);

        return token;
    }
}