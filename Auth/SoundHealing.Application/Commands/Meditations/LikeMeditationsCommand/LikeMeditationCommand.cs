using CQRS;
using MediatR;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LikeMeditationsCommand;

public record LikeMeditationCommand(Guid userId, Guid meditationId) : IRequest<Result<bool>>;

public class LikeMeditationCommandHandler(IUserRepository userRepository) 
    : IRequestHandler<LikeMeditationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(LikeMeditationCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.SetLikeToMeditationAsync(
            request.userId,
            request.meditationId,
            cancellationToken);
    }
}