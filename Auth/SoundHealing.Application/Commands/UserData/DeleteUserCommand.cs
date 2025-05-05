using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.UserData;

public record DeleteUserCommand(Guid UserId) : IRequest<Result<Unit>>;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUserCredentialsRepository userCredentialsRepository) : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsyncWithoutIncludes(request.UserId, cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        await userRepository.DeleteByIdAsync(request.UserId, cancellationToken);
        await userCredentialsRepository.DeleteByUserIdAsync(request.UserId, cancellationToken);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        await userCredentialsRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}