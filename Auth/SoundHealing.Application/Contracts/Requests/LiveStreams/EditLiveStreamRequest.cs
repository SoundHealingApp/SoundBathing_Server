namespace SoundHealing.Application.Contracts.Requests.LiveStreams;

public record EditLiveStreamRequest(
    string? Title,
    string? Description,
    string? TherapeuticPurpose,
    string? StartDateTime,
    string? YouTubeUrl);