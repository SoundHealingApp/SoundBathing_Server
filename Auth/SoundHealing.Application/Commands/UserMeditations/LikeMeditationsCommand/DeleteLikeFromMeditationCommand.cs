using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.UserMeditations.LikeMeditationsCommand;

public record DeleteLikeFromMeditationCommand(Guid UserId, Guid MeditationId) : IRequest<Result<Unit>>;

public class DeleteLikeFromMeditationCommandHandler(
    IUserRepository userRepository, IMediationRepository mediationRepository) 
    : IRequestHandler<DeleteLikeFromMeditationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteLikeFromMeditationCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        var meditation = await mediationRepository.GetByIdAsync(
            request.MeditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExistsError(request.MeditationId);
        
        user.DeleteLikeFromMeditation(meditation);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}