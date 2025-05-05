using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Core.Enums;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations;

public record GetMeditationsCommand(MeditationType MeditationType)
    : IRequest<Result<List<Core.Models.Meditation>>>;

public class GetMeditationsCommandHandler(IMediationRepository mediationRepository)
    : IRequestHandler<GetMeditationsCommand, Result<List<Core.Models.Meditation>>>
{
    public async Task<Result<List<Core.Models.Meditation>>> Handle(
        GetMeditationsCommand request,
        CancellationToken cancellationToken)
    {
        var meditations =
            await mediationRepository.GetByTypeAsync(request.MeditationType, cancellationToken);

        if (meditations == null || meditations.Count == 0)
            return new MeditationsDoesNotExists();

        return meditations;
    }
}