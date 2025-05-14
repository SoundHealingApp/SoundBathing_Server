using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Meditations.FeedbackCommands;

public record GetMeditationFeedbacksCommand(Guid MeditationId) : IRequest<Result<List<MeditationFeedback>>>;

internal sealed class GetMeditationFeedbacksCommandHandler(IMediationRepository mediationRepository)
    : IRequestHandler<GetMeditationFeedbacksCommand, Result<List<MeditationFeedback>>> 
{
    public async Task<Result<List<MeditationFeedback>>> Handle(GetMeditationFeedbacksCommand request, CancellationToken cancellationToken)
    {
        var meditation = await mediationRepository.GetByIdAsync(request.MeditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExistsError(request.MeditationId);

        return meditation.Feedbacks;
    }
}