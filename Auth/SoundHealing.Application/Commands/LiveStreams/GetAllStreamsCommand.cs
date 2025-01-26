using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;


public record GetAllStreamsCommand : IRequest<Result<List<LiveStreamDto>>>;

internal sealed class GetAllStreamsCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<GetAllStreamsCommand, Result<List<LiveStreamDto>>>
{
    public async Task<Result<List<LiveStreamDto>>> Handle(GetAllStreamsCommand request, CancellationToken cancellationToken)
    {
        var streams = await liveStreamRepository.GetSortedStreamsAsync(cancellationToken);
        
        var streamDtos = streams.Select(stream => 
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