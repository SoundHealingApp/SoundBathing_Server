using SoundHealing.Extensions;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;

public record GetPastStreamsCommand : IRequest<Result<List<LiveStreamDto>>>;

internal sealed class GetPastStreamsCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<GetPastStreamsCommand, Result<List<LiveStreamDto>>>
{
    public async Task<Result<List<LiveStreamDto>>> Handle(GetPastStreamsCommand request, CancellationToken cancellationToken)
    {
        // Получаем часовой пояс Лондона
        var londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        // Получаем текущее время в Лондоне (с учетом текущего времени и летнего/зимнего времени)
        var currentLondonTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, londonTimeZone);
        
        var streams = await liveStreamRepository.GetSortedStreamsAsync(cancellationToken);
        
        var streamDtos = streams
            .Where(x => x.StartDateTime.AddMinutes(5) < currentLondonTime)
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