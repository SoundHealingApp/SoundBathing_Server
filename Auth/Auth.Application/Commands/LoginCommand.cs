using Auth.Application.Errors;
using Auth.Application.Interfaces;
using Auth.Core.Interfaces;
using CQRS;
using MediatR;

namespace Auth.Application.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;

public class LoginCommandHandler(
    IUserCredentialsRepository userCredentialsRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userCredentialsRepository.GetByEmailAsync(request.Email);

        if (user == null)
            return new UserNotFoundError(request.Email);
        
        var result = passwordHasher.Verify(request.Password, user.PasswordHash);
        
        if (!result)
            return new IncorrectPasswordError();

        var token = jwtProvider.GenerateToken(user);

        return token;
    }
}