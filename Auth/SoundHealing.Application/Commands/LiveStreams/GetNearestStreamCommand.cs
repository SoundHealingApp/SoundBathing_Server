using CQRS;
using MediatR;
using SoundHealing.Application.Contracts.Dto;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Application.Commands.LiveStreams;

public record GetNearestStreamCommand : IRequest<Result<LiveStreamDto>?>;

internal sealed class GetNearestStreamCommandHandler(ILiveStreamRepository liveStreamRepository)
    : IRequestHandler<GetNearestStreamCommand, Result<LiveStreamDto>?>
{
    public async Task<Result<LiveStreamDto>?> Handle(GetNearestStreamCommand request, CancellationToken cancellationToken)
    {
        // Получаем часовой пояс Лондона
        var londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        // Получаем текущее время в Лондоне (с учетом текущего времени и летнего/зимнего времени)
        var currentLondonTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, londonTimeZone);

        var streams = await liveStreamRepository.GetSortedStreamsAsync(cancellationToken);
        
        var upcomingStream = streams.FirstOrDefault(x => x.StartDateTime >= currentLondonTime.AddMinutes(-5));

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