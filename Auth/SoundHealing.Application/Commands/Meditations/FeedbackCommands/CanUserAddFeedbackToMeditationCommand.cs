using CQRS;
using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.FeedbackCommands;

public record CanUserAddFeedbackToMeditationCommand(Guid UserId, Guid MeditationId) : IRequest<Result<bool>>;

internal sealed class CanUserAddFeedbackToMeditationCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CanUserAddFeedbackToMeditationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CanUserAddFeedbackToMeditationCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());

        var isFeedbackExists = user.MeditationFeedbacks.Any(x => x.MeditationId == request.MeditationId);

        return !isFeedbackExists;
    }
}