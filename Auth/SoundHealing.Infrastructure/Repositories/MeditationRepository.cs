using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class MeditationRepository(UserDbContext userDbContext) : IMediationRepository
{
    public Task AddAsync(Meditation meditation, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}