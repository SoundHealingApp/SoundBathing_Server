using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;


public record GetUpcomingStreamsCommand : IRequest<Result<List<LiveStreamDto>>>;

internal sealed class GetUpcomingStreamsCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<GetUpcomingStreamsCommand, Result<List<LiveStreamDto>>>
{
    public async Task<Result<List<LiveStreamDto>>> Handle(GetUpcomingStreamsCommand request, CancellationToken cancellationToken)
    {
        var streams = await liveStreamRepository.GetSortedStreamsAsync(cancellationToken);
        
        var streamDtos = streams
            .Where(x => x.StartDateTime >= DateTime.UtcNow.AddMinutes(-5))
            .Select(stream => 
                new LiveStreamDto(
                    stream.Id,
                    stream.Title,
                    stream.Description,
                    stream.TherapeuticPurpose,
                    stream.StartDateTime,
                    stream.YouTubeUrl))
                .ToList();

        return streamDtos;
    }
}