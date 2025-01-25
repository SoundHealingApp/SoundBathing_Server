using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.UserMeditations.RecommendMeditationsCommands;

public record RecommendMeditationCommand(Guid UserId, List<Guid> MeditationsIds) : IRequest<Result<Unit>>;

internal sealed class RecommendMeditationsCommandHandler(
    IUserRepository userRepository, IMediationRepository mediationRepository)
    : IRequestHandler<RecommendMeditationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RecommendMeditationCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var meditations = await mediationRepository
            .GetMeditationsWithIdsAsync(request.MeditationsIds, cancellationToken);
        
        if (meditations.Count == 0)
            return new GivenMeditationsDoesNotExists(request.MeditationsIds);
        
        user.AddRecommendedMeditations(meditations);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}