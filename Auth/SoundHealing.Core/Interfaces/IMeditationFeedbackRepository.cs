using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IMeditationFeedbackRepository
{
    public Task AddAsync(MeditationFeedback feedback, CancellationToken cancellationToken);
    
    public Task DeleteAsync(Guid meditationId, Guid userId, CancellationToken cancellationToken);
    
    public Task ChangeFeedbackAsync(Guid meditationId, Guid userId, string? comment, int estimate, CancellationToken cancellationToken);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}