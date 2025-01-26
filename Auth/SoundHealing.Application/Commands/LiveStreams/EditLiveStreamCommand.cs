using CQRS;
using MediatR;
using SoundHealing.Application.Errors.LiveStreamErrors;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;

public record EditLiveStreamCommand(
    Guid LiveStreamId,
    string? Title,
    string? Description,
    string? TherapeuticPurpose,
    DateTime? StartDateTime,
    string? YouTubeUrl) : IRequest<Result<Unit>>;
    
internal sealed class EditLiveStreamCommandHandler(ILiveStreamRepository liveStreamRepository) : IRequestHandler<EditLiveStreamCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(EditLiveStreamCommand request, CancellationToken cancellationToken)
    {
        var liveStream = await liveStreamRepository.GetAsync(request.LiveStreamId, cancellationToken);

        if (liveStream == null)
            return new LiveStreamWithIdDoesNotExistsError(request.LiveStreamId);
        
        if (request.Title != null)
            liveStream.ChangeTitle(request.Title);
        
        if (request.Description != null)
            liveStream.ChangeDescription(request.Description);
        
        if (request.TherapeuticPurpose != null)
            liveStream.ChangeTherapeuticPurpose(request.TherapeuticPurpose);
        
        if (request.StartDateTime != null)
            liveStream.ChangeStartDateTime(request.StartDateTime!.Value);
        
        if (request.YouTubeUrl != null)
            liveStream.ChangeYouTubeUrl(request.YouTubeUrl);
        
        await liveStreamRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}