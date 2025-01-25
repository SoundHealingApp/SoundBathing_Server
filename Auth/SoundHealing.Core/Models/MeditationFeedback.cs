namespace SoundHealing.Core.Models;

public class MeditationFeedback(Guid userId, Guid meditationId, string? comment, int estimate)
{
    public Guid UserId { get; set; } = userId;

    public Guid MeditationId { get; set; } = meditationId;

    public string? Comment { get; set; } = comment;

    public int Estimate { get; set; } = estimate;
}