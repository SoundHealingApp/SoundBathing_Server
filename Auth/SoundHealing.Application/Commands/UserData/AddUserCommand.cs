using CQRS;
using MediatR;
using SoundHealing.Application.Errors.AuthErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.UserData;

public record AddUserCommand(string UserId, string Name, string Surname, DateOnly BirthDate)
    : IRequest<Result<Unit>>;

internal sealed class AddUserCommandHandler(IUserRepository userRepository, IPermissionRepository permissionRepository)
    : IRequestHandler<AddUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user != null)
            return new UserAlreadyExistsError(request.UserId);
        
        user = new User(request.UserId, request.Name, request.Surname, request.BirthDate);
        
        var userPermissions = await permissionRepository.GetUserPermissionsAsync(cancellationToken);
        user.AssignUserPermissions(userPermissions);
        
        await userRepository.AddAsync(user, cancellationToken);
        
        await userRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}