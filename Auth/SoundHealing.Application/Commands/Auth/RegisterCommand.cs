using CQRS;
using MediatR;
using SoundHealing.Application.Errors.Auth;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Auth;

public record RegisterCommand(string Email, string Password) : IRequest<Result<(string Token, string UserId)>>;

internal sealed class RegisterCommandHandler(
    IUserCredentialsRepository userCredentialsRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IRequestHandler<RegisterCommand, Result<(string Token, string UserId)>>
{
    public async Task<Result<(string Token, string UserId)>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userCredentialsRepository.GetByEmailAsync(request.Email) != null)
            return new UserAlreadyExistsError(request.Email);
        
        var hashedPassword = passwordHasher.Generate(request.Password);
        
        var userCredentials = UserCredentials.Create(request.Email, hashedPassword);
        
        await userCredentialsRepository.AddAsync(userCredentials);
        
        var token = jwtProvider.GenerateToken(userCredentials);

        return(token, userCredentials.Id.ToString());
    }
}