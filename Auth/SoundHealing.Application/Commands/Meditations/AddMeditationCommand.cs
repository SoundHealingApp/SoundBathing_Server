using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Requests.Meditation;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.S3Errors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Meditations;

public record AddMeditationCommand(AddMeditationRequest AddMeditationRequest) : IRequest<Result<Unit>>;

internal sealed class AddMeditationCommandHandler(
    IMediationRepository mediationRepository,
    IS3Repository s3Repository) : IRequestHandler<AddMeditationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AddMeditationCommand request, CancellationToken cancellationToken)
    {
        var meditationDto = request.AddMeditationRequest;
        
        var isMeditationExists = await mediationRepository.IsMeditationExistsAsync(
            meditationDto.Title, cancellationToken);
        
        if (isMeditationExists)
            return new MeditationAlreadyExistsError(meditationDto.Title);
        
        var meditation = new Meditation(
            meditationDto.Title,
            meditationDto.Description,
            meditationDto.MeditationType,
            meditationDto.TherapeuticPurpose,
            meditationDto.Frequency);
        
        var imageKey = await s3Repository.AddMeditationImageAsync(meditationDto.Image, meditation.Id);
        
        if (imageKey == null)
            return new S3ImageUploadError(meditation.Id);
        
        var audioKey = await s3Repository.AddMeditationAudioAsync(meditationDto.Audio, meditation.Id);
        
        if (audioKey == null)
            return new S3AudioUploadError(meditation.Id);
        
        meditation.SetS3Keys(imageKey, audioKey);
        
        // Если при сохранении в бд что-то пойдет не так, то в s3 ключи уже все равно будут 
        await mediationRepository.AddAsync(meditation, cancellationToken);

        return Unit.Value;
    }
}