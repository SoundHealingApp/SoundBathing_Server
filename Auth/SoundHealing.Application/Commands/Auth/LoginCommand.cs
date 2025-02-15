using CQRS;
using MediatR;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Auth;
public record LoginResponse(string Token, string UserId);

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public class LoginCommandHandler(
    IUserCredentialsRepository userCredentialsRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var userCredentials = await userCredentialsRepository.GetByEmailAsync(request.Email);

        if (userCredentials == null)
            return new UserWithEmailNotFoundError(request.Email);
        
        var result = passwordHasher.Verify(request.Password, userCredentials.PasswordHash);
        
        if (!result)
            return new IncorrectPasswordError();

        var token = jwtProvider.GenerateToken(userCredentials);

        return new LoginResponse(Token: token, UserId: userCredentials.Id.ToString());
    }
}