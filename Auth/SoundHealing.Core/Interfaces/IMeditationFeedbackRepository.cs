using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IMeditationFeedbackRepository
{
    public Task AddAsync(MeditationFeedback feedback, CancellationToken cancellationToken);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}