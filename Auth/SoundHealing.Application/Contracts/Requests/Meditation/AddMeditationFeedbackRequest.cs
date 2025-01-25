namespace SoundHealing.Application.Contracts.Requests.Meditation;

public record AddMeditationFeedbackRequest(Guid UserId, string? Comment, int Estimate);