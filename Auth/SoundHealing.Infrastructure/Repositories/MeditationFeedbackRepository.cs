using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationFeedbackRepository(AppDbContext appDbContext) : IMeditationFeedbackRepository
{
    public async Task AddAsync(MeditationFeedback feedback, CancellationToken cancellationToken)
    {
        await appDbContext.MeditationsFeedback.AddAsync(feedback, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => appDbContext.SaveChangesAsync(cancellationToken);
}