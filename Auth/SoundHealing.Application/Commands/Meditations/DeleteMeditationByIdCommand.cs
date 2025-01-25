using CQRS;
using MediatR;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations;

public record DeleteMeditationByIdCommand(Guid meditationId) : IRequest<Result<Unit>>;

internal sealed class DeleteMeditationByIdCommandHandler(
    IMediationRepository mediationRepository,
    IS3Repository s3Repository) : IRequestHandler<DeleteMeditationByIdCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteMeditationByIdCommand request, CancellationToken cancellationToken)
    {
        var meditation = await mediationRepository.GetByIdAsync(request.meditationId, cancellationToken);
        
        if (meditation == null)
            return new MeditationWithIdDoesNotExistsError(request.meditationId);
        
        await mediationRepository.DeleteAsync(meditation, cancellationToken);

        await s3Repository.DeleteFileAsync(meditation.ImageLink);
        await s3Repository.DeleteFileAsync(meditation.AudioLink);

        return Unit.Value;
    }
}