using CQRS;
using MediatR;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations.LikeMeditationsCommand;

public record DeleteLikeFromMeditationCommand(Guid userId, Guid meditationId) : IRequest<Result<bool>>;

public class DeleteLikeFromMeditationCommandHandler(IUserRepository userRepository) 
    : IRequestHandler<DeleteLikeFromMeditationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteLikeFromMeditationCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.DeleteLikeFromMeditationAsync(
            request.userId,
            request.meditationId,
            cancellationToken);
    }
}