using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;
using SoundHealing.Extensions;

namespace SoundHealing.Application.Commands.UserMeditations.LikeMeditationsCommand;

public record GetLikedMeditationsCommand(Guid UserId) : IRequest<Result<List<Meditation>>>;

public class GetLikedMeditationsCommandHandler(IUserRepository userRepository)
    : IRequestHandler<GetLikedMeditationsCommand, Result<List<Meditation>>>
{
    public async Task<Result<List<Meditation>>> Handle(
        GetLikedMeditationsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());

        return user.LikedMeditations.ToList();
    }
}