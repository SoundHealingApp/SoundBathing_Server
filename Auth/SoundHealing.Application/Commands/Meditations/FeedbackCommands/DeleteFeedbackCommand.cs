using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.FeedbackCommands;

public record DeleteFeedbackCommand(Guid UserId, Guid MeditationId) : IRequest<Result<Unit>>;

internal sealed class DeleteMeditationFeedbackCommandHandler(
    IUserRepository userRepository,
    IMediationRepository mediationRepository, 
    IMeditationFeedbackRepository feedbackRepository) : IRequestHandler<DeleteFeedbackCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var meditation = await mediationRepository.GetByIdAsync(request.MeditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExistsError(request.MeditationId);

        // if (meditation.Feedbacks.All(x => x.UserId != request.UserId))
        // {
        //     
        // }

        await feedbackRepository.DeleteAsync(meditationId: meditation.Id, userId: user.Id, cancellationToken);
        
        await feedbackRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}