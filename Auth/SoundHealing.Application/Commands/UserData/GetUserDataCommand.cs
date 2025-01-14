using CQRS;
using MediatR;
using SoundHealing.Application.Errors.Auth;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.UserData;

public record GetUserDataCommand(string UserId) : IRequest<Result<User>>;

public class GetUserDataCommandHandler(IUserRepository userRepository) : IRequestHandler<GetUserDataCommand, Result<User>>
{
    public async Task<Result<User>> Handle(GetUserDataCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId);

        return user;
    }
}