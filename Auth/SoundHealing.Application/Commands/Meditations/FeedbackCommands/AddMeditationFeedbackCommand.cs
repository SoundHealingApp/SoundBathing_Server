using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Meditations.FeedbackCommands;

public record AddMeditationFeedbackCommand(Guid UserId, Guid MeditationId, string? Comment, int Estimate)
    : IRequest<Result<Unit>>;

internal sealed class AddMeditationFeedbackCommandHandler(
    IUserRepository userRepository,
    IMediationRepository mediationRepository, 
    IMeditationFeedbackRepository feedbackRepository)
    : IRequestHandler<AddMeditationFeedbackCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AddMeditationFeedbackCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var meditation = await mediationRepository.GetByIdAsync(request.MeditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExistsError(request.MeditationId);
        
        if (meditation.Feedbacks.Any(x => x.UserId == request.UserId))
            return new UserAlreadyProvidedFeedbackError(request.UserId, request.MeditationId);
        
        await feedbackRepository.AddAsync(
            new MeditationFeedback(
                request.UserId, request.MeditationId, request.Comment, request.Estimate),
            cancellationToken);
        
        meditation.SetRating();
        
        await feedbackRepository.SaveChangesAsync(cancellationToken);
        await mediationRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}