using SoundHealing.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Application.Errors.S3Errors;
using SoundHealing.Core.Enums;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.Meditations;

public record EditMeditationCommand(
    Guid meditationId,
    string? Title,
    string? Description,
    MeditationType? MeditationType,
    string? TherapeuticPurpose,
    IFormFile? Image,
    IFormFile? Audio,
    double? Frequency) : IRequest<Result<Unit>>;

internal sealed class EditMeditationCommandHandler(
    IMediationRepository mediationRepository,
    IS3Repository s3Repository) : IRequestHandler<EditMeditationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(EditMeditationCommand request, CancellationToken cancellationToken)
    {
        var meditation = await mediationRepository.GetByIdAsync(request.meditationId, cancellationToken);
        
        if (meditation is null)
            return new MeditationWithIdDoesNotExistsError(request.meditationId);
        
        if (request.Title is not null)
            meditation.SetTitle(request.Title);
        
        if (request.Description is not null)
            meditation.SetDescription(request.Description);
        
        if (request.MeditationType is not null)
            meditation.SetMeditationType(request.MeditationType.Value);
        
        if (request.TherapeuticPurpose is not null)
            meditation.SetTherapeuticPurpose(request.TherapeuticPurpose);
        
        if (request.Frequency is not null)
            meditation.SetFrequency(request.Frequency.Value);
        

        if (request.Image is not null)
        {
            await s3Repository.DeleteFileAsync(meditation.ImageLink);
            
            var imageKey = await s3Repository.AddMeditationImageAsync(request.Image, meditation.Id);

            if (imageKey == null)
                return new S3ImageUploadError(meditation.Id);
            
            meditation.SetImageKey(imageKey);
        }
        
        if (request.Audio is not null)
        {
            await s3Repository.DeleteFileAsync(meditation.AudioLink);
            
            var audioKey = await s3Repository.AddMeditationAudioAsync(request.Audio, meditation.Id);

            if (audioKey == null)
                return new S3AudioUploadError(meditation.Id);
            
            meditation.SetAudioKey(audioKey);
        }

        await mediationRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}