using SoundHealing.Core.Enums;
using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IMediationRepository
{
    public Task AddAsync(
        Meditation meditation,
        CancellationToken cancellationToken);
    
    public Task DeleteAsync(
        Meditation meditation,
        CancellationToken cancellationToken);
    
    public Task<bool> IsExistsAsync(
        string title,
        CancellationToken cancellationToken);

    public Task<List<Meditation>?> GetByTypeAsync(
        MeditationType meditationType,
        CancellationToken cancellationToken);

    public Task<Meditation?> GetByIdAsync(
        Guid meditationId,
        CancellationToken cancellationToken);
    
    public Task<List<Meditation>> GetMeditationsWithIdsAsync(
        List<Guid> meditationsIds,
        CancellationToken cancellationToken);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}