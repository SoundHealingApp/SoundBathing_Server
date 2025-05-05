using MediatR;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;
using SoundHealing.Extensions;

namespace SoundHealing.Application.Commands.Auth;

public record RegisterResponse(string Token, string UserId);
public record RegisterCommand(string Email, string Password) : IRequest<Result<RegisterResponse>>;

internal sealed class RegisterCommandHandler(
    IUserCredentialsRepository userCredentialsRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userCredentialsRepository.GetByEmailAsync(request.Email) != null)
            return new UserAlreadyExistsError(request.Email);
        
        var hashedPassword = passwordHasher.Generate(request.Password);
        
        var userCredentials = UserCredentials.Create(request.Email, hashedPassword);
        
        await userCredentialsRepository.AddAsync(userCredentials);
        
        // var token = jwtProvider.GenerateToken(userCredentials);

        return new RegisterResponse(Token: "token", UserId: userCredentials.Id.ToString());
    }
}