using SoundHealing.Core.Enums;
using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IMediationRepository
{
    public Task AddAsync(Meditation meditation, CancellationToken cancellationToken);
    
    public Task<bool> IsMeditationExistsAsync(string title, CancellationToken cancellationToken);

    public Task<List<Meditation>?> GetMeditationsByTypeAsync(
        MeditationType meditationType,
        CancellationToken cancellationToken);

    public Task<Meditation?> GetMeditationByIdAsync(Guid meditationId, CancellationToken cancellationToken);
}