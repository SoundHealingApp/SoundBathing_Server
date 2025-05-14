using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.FeedbackCommands;

public record ChangeFeedbackCommand(
    Guid MeditationId,
    Guid UserId,
    string? Comment,
    int Estimate) : IRequest<Result<Unit>>;

internal sealed class ChangeFeedbackCommandHandler(
    IUserRepository userRepository,
    IMediationRepository mediationRepository,
    IMeditationFeedbackRepository meditationFeedbackRepository) : IRequestHandler<ChangeFeedbackCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(
        ChangeFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var meditation = await mediationRepository.GetByIdAsync(request.MeditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExistsError(request.MeditationId);

        await meditationFeedbackRepository.ChangeFeedbackAsync(
            request.MeditationId,
            request.UserId,
            request.Comment,
            request.Estimate,
            cancellationToken);
        
        return Unit.Value;
    }
}