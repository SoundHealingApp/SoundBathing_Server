using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationFeedbackRepository(UserDbContext userDbContext) : IMeditationFeedbackRepository
{
    public async Task AddAsync(MeditationFeedback feedback, CancellationToken cancellationToken)
    {
        await userDbContext.MeditationsFeedback.AddAsync(feedback, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => userDbContext.SaveChangesAsync(cancellationToken);
}