using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface ILiveStreamRepository
{
    public Task AddAsync(LiveStream liveStream, CancellationToken cancellationToken);
    
    public Task<LiveStream?> GetAsync(Guid liveStreamId, CancellationToken cancellationToken);
    
    public Task<List<LiveStream>> GetSortedStreamsAsync(CancellationToken cancellationToken);
    
    public Task DeleteAsync(LiveStream liveStream, CancellationToken cancellationToken);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}