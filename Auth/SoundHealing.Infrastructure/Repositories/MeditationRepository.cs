using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Enums;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationRepository(UserDbContext userDbContext) : IMediationRepository
{
    public async Task AddAsync(Meditation meditation, CancellationToken cancellationToken)
    {
        // TODO: добавлять медитации в s3
        
        await userDbContext.Meditations.AddAsync(meditation, cancellationToken);
        await userDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsMeditationExistsAsync(string title, CancellationToken cancellationToken)
    {
        var meditation = userDbContext.Meditations
            .AsNoTracking()
            .FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
        
        return Task.FromResult(meditation != null);
    }

    public async Task<List<Meditation>?> GetMeditationsByTypeAsync(
        MeditationType meditationType,
        CancellationToken cancellationToken)
    {
        var meditations = await userDbContext.Meditations
            .AsNoTracking()
            .Where(x => x.MeditationType == meditationType)
            .ToListAsync(cancellationToken: cancellationToken);

        return meditations;
    }

    public async Task<Meditation?> GetMeditationByIdAsync(Guid meditationId, CancellationToken cancellationToken)
    {
        var meditation = await userDbContext.Meditations
            .AsNoTracking()
            .Where(x => x.Id == meditationId)
            .FirstOrDefaultAsync(cancellationToken);

        return meditation;
    }
}