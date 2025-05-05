using MediatR;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;
using SoundHealing.Extensions;

namespace SoundHealing.Application.Commands.LiveStreams;

public record AddLiveStreamCommand(
    string Title,
    string Description,
    string? TherapeuticPurpose,
    DateTime StartDateTime,
    string YouTubeUrl) : IRequest<Result<Unit>>;

internal sealed class AddLiveStreamCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<AddLiveStreamCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AddLiveStreamCommand request, CancellationToken cancellationToken)
    {
        var liveStream = new LiveStream(
            request.Title,
            request.Description,
            request.TherapeuticPurpose,
            request.StartDateTime,
            request.YouTubeUrl);
        
        await liveStreamRepository.AddAsync(liveStream, cancellationToken);
        
        await liveStreamRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}