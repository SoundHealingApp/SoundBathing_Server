using CQRS;
using MediatR;
using SoundHealing.Application.Errors.UsersErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.UserMeditations.RecommendMeditationsCommands;

public record GetRecommendedMeditationsCommand(Guid UserId) : IRequest<Result<List<Meditation>>>;

internal sealed class GetRecommendedMeditationsCommandHandler(IUserRepository userRepository)
    : IRequestHandler<GetRecommendedMeditationsCommand, Result<List<Meditation>>>
{
    public async Task<Result<List<Meditation>>> Handle(GetRecommendedMeditationsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return new UserWithIdNotFoundError(request.UserId.ToString());
        
        return user.RecommendedMeditations.ToList();
    }
}