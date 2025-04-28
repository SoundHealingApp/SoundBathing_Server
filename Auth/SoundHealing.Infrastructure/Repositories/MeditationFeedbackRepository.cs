using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationFeedbackRepository(AppDbContext appDbContext) : IMeditationFeedbackRepository
{
    public async Task AddAsync(MeditationFeedback feedback, CancellationToken cancellationToken)
    {
        await appDbContext.MeditationsFeedback.AddAsync(feedback, cancellationToken);
    }

    public async Task DeleteAsync(Guid meditationId, Guid userId, CancellationToken cancellationToken)
    {
        await appDbContext.MeditationsFeedback
            .Where(x => x.MeditationId == meditationId)
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task ChangeFeedbackAsync(
        Guid meditationId,
        Guid userId,
        string? comment,
        int estimate,
        CancellationToken cancellationToken)
    {
        var feedback = await appDbContext.MeditationsFeedback
            .Where(x => x.MeditationId == meditationId)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (feedback != null)
        {
            feedback.Comment = comment;
            feedback.Estimate = estimate;
        }

        await SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => appDbContext.SaveChangesAsync(cancellationToken);
}