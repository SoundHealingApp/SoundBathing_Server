using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Meditations;

public record GetMeditationInfoByIdCommand(Guid meditationId) : IRequest<Result<Meditation>>;

internal sealed class GetMeditationInfoByIdCommandHandler(IMediationRepository mediationRepository)
    : IRequestHandler<GetMeditationInfoByIdCommand, Result<Meditation>>
{
    public async Task<Result<Meditation>> Handle(
        GetMeditationInfoByIdCommand request,
        CancellationToken cancellationToken)
    {
        var meditation = await mediationRepository
            .GetByIdAsync(request.meditationId, cancellationToken);

        if (meditation == null)
            return new MeditationWithIdDoesNotExists(request.meditationId);

        return meditation;
    }
}