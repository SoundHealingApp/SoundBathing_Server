using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.LikeMeditationsCommand;

public record LikeMeditationCommand(Guid UserId, Guid MeditationId) : IRequest<Result<Unit>>;

public class LikeMeditationCommandHandler(IUserRepository userRepository, IMediationRepository mediationRepository) 
    : IRequestHandler<LikeMeditationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(LikeMeditationCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var meditation = await mediationRepository.GetByIdAsync(
            request.MeditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExists(request.MeditationId);
        
        user.SetLikeToMeditation(meditation);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}