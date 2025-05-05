using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.UserData;

public record CheckUserPermissionsCommand(Guid UserId, string PermissionName) : IRequest<Result<bool>>;

internal sealed class CheckUserPermissionsCommandHandler(
    IUserRepository userRepository,
    IPermissionRepository permissionRepository)
    : IRequestHandler<CheckUserPermissionsCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(
        CheckUserPermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsyncWithoutIncludes(request.UserId, cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var result = await permissionRepository.CheckUserHasPermissionAsync(
            request.UserId,
            request.PermissionName,
            cancellationToken);

        return result;
    }
}