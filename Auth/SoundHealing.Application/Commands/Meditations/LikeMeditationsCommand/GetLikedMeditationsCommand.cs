using CQRS;
using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Meditations.LikeMeditationsCommand;

public record GetLikedMeditationsCommand(Guid userId) : IRequest<Result<List<Meditation>>>;

public class GetLikedMeditationsCommandHandler(IUserRepository userRepository)
    : IRequestHandler<GetLikedMeditationsCommand, Result<List<Meditation>>>
{
    public async Task<Result<List<Meditation>>> Handle(
        GetLikedMeditationsCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.GetLikedMeditationsAsync(request.userId, cancellationToken);
    }
}