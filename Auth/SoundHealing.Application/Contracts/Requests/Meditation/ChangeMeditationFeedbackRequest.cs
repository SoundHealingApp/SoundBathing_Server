namespace SoundHealing.Application.Contracts.Requests.Meditation;

public record ChangeMeditationFeedbackRequest(
    string? Comment,
    int Estimate);