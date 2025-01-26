using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Enums;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationRepository(UserDbContext userDbContext) : IMediationRepository
{
    public async Task AddAsync(Meditation meditation, CancellationToken cancellationToken)
    {
        await userDbContext.Meditations.AddAsync(meditation, cancellationToken);
        await userDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(Meditation meditation, CancellationToken cancellationToken)
    {
        userDbContext.Meditations.Remove(meditation);
        
        await userDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsExistsAsync(string title, CancellationToken cancellationToken)
    {
        var meditation = userDbContext.Meditations
            .AsNoTracking()
            .FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
        
        return Task.FromResult(meditation != null);
    }

    public async Task<List<Meditation>?> GetByTypeAsync(
        MeditationType meditationType,
        CancellationToken cancellationToken)
    {
        var meditations = await userDbContext.Meditations
            .AsNoTracking()
            .Include(x => x.Feedbacks)
            .Where(x => x.MeditationType == meditationType)
            .ToListAsync(cancellationToken: cancellationToken);

        return meditations;
    }

    public async Task<Meditation?> GetByIdAsync(Guid meditationId, CancellationToken cancellationToken)
    {
        var meditation = await userDbContext.Meditations
            // .AsNoTracking()
            .Include(x => x.Feedbacks)
            .Where(x => x.Id == meditationId)
            .FirstOrDefaultAsync(cancellationToken);

        return meditation;
    }

    public async Task<List<Meditation>> GetMeditationsWithIdsAsync(
        List<Guid> meditationsIds,
        CancellationToken cancellationToken)
    {
        var meditations = await userDbContext.Meditations
            // .AsNoTracking()
            .Where(x => meditationsIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return meditations;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        userDbContext.SaveChangesAsync(cancellationToken);
}