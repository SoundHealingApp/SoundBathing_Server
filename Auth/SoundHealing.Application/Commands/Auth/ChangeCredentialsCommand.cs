using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;
using SoundHealing.Extensions;

namespace SoundHealing.Application.Commands.Auth;

public record ChangeCredentialsCommand(Guid UserId, string? Email, string? Password) : IRequest<Result<Unit>>;

internal sealed class ChangeCredentialsCommandHandler(
    IUserCredentialsRepository userCredentialsRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<ChangeCredentialsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ChangeCredentialsCommand request, CancellationToken cancellationToken)
    {
        var userCredentials = await userCredentialsRepository.GetByUserIdAsync(request.UserId);

        if (userCredentials == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        if (request.Email != null)
            userCredentials.ChangeEmail(request.Email);

        if (request.Password != null)
        {
            var hashedPassword = passwordHasher.Generate(request.Password);
            userCredentials.ChangePasswordHash(hashedPassword);
        }

        await userCredentialsRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}