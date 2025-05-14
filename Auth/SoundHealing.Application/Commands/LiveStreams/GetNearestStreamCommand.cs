using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;

public record GetNearestStreamCommand : IRequest<Result<LiveStreamDto?>>;

internal sealed class GetNearestStreamCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<GetNearestStreamCommand, Result<LiveStreamDto?>>
{
    public async Task<Result<LiveStreamDto?>> Handle(GetNearestStreamCommand request, CancellationToken cancellationToken)
    {
        var streams = await liveStreamRepository.GetSortedStreamsAsync(cancellationToken);
        LiveStreamDto? streamDto = null;
        
        var upcomingStream = streams.FirstOrDefault(x => x.StartDateTime >= DateTime.UtcNow.AddMinutes(-5));

        if (upcomingStream == null)
            return streamDto;
        
        streamDto = new LiveStreamDto(
            upcomingStream.Id,
            upcomingStream.Title,
            upcomingStream.Description,
            upcomingStream.TherapeuticPurpose,
            upcomingStream.StartDateTime,
            upcomingStream.YouTubeUrl);

        return streamDto;
    }
}