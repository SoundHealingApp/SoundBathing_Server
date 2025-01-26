using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class LiveStreamRepository(UserDbContext dbContext) : ILiveStreamRepository
{
    public async Task AddAsync(LiveStream liveStream, CancellationToken cancellationToken) =>
        await dbContext.LiveStreams.AddAsync(liveStream, cancellationToken);

    public async Task<LiveStream?> GetAsync(Guid liveStreamId, CancellationToken cancellationToken)
    {
        return await dbContext.LiveStreams
            .Where(x => x.Id == liveStreamId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LiveStream>> GetSortedStreamsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.LiveStreams
            .OrderBy(x => x.StartDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(LiveStream liveStream, CancellationToken cancellationToken)
    {
        dbContext.LiveStreams.Remove(liveStream);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}