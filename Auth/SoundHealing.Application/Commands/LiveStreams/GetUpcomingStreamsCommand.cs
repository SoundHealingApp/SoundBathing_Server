using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;

public record GetUpcomingStreamsCommand : IRequest<Result<LiveStreamDto>?>;

internal sealed class GetUpcomingStreamsCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<GetUpcomingStreamsCommand, Result<LiveStreamDto>?>
{
    public async Task<Result<LiveStreamDto>?> Handle(GetUpcomingStreamsCommand request, CancellationToken cancellationToken)
    {
        var streams = await liveStreamRepository.GetSortedStreamsAsync(cancellationToken);
        
        var upcomingStream = streams.FirstOrDefault(x => x.StartDateTime >= DateTime.UtcNow);

        if (upcomingStream == null) return null;
        
        var streamDto = new LiveStreamDto(
            upcomingStream.Id,
            upcomingStream.Title,
            upcomingStream.Description,
            upcomingStream.TherapeuticPurpose,
            upcomingStream.StartDateTime,
            upcomingStream.YouTubeUrl);

        return streamDto;

    }
}