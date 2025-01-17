using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Requests.Meditation;
using SoundHealing.Application.Errors.MeditationErrors;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Application.Commands.Meditations;

public record AddMeditationCommand(AddMeditationRequest AddMeditationRequest) : IRequest<Result<Unit>>;

internal sealed class AddMeditationCommandHandler(IMediationRepository mediationRepository)
    : IRequestHandler<AddMeditationCommand, Result<Unit>>
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
            meditationDto.ImageLink, 
            meditationDto.VideoLink,
            meditationDto.Frequency);
        
        await mediationRepository.AddAsync(meditation, cancellationToken);

        return Unit.Value;
    }
}