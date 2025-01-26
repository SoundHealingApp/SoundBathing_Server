using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Enums;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationRepository(AppDbContext appDbContext) : IMediationRepository
{
    public async Task AddAsync(Meditation meditation, CancellationToken cancellationToken)
    {
        await appDbContext.Meditations.AddAsync(meditation, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(Meditation meditation, CancellationToken cancellationToken)
    {
        appDbContext.Meditations.Remove(meditation);
        
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsExistsAsync(string title, CancellationToken cancellationToken)
    {
        var meditation = appDbContext.Meditations
            .AsNoTracking()
            .FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
        
        return Task.FromResult(meditation != null);
    }

    public async Task<List<Meditation>?> GetByTypeAsync(
        MeditationType meditationType,
        CancellationToken cancellationToken)
    {
        var meditations = await appDbContext.Meditations
            .AsNoTracking()
            .Include(x => x.Feedbacks)
            .Where(x => x.MeditationType == meditationType)
            .ToListAsync(cancellationToken: cancellationToken);

        return meditations;
    }

    public async Task<Meditation?> GetByIdAsync(Guid meditationId, CancellationToken cancellationToken)
    {
        var meditation = await appDbContext.Meditations
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
        var meditations = await appDbContext.Meditations
            // .AsNoTracking()
            .Where(x => meditationsIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return meditations;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        appDbContext.SaveChangesAsync(cancellationToken);
}