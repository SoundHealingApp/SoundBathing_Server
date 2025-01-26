namespace SoundHealing.Application.Contracts.Requests.LiveStreams;

public record AddLiveStreamRequest(
    string Title,
    string Description,
    string? TherapeuticPurpose,
    string StartDateTime,
    string YouTubeUrl);