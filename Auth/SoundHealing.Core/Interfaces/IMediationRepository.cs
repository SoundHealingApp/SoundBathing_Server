using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IMediationRepository
{
    public Task AddAsync(Meditation meditation, CancellationToken cancellationToken);
}