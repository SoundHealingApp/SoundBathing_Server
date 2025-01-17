using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IUserRepository
{
    public Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
    
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    public Task<bool> SetLikeToMeditationAsync(Guid userId, Guid meditationId, CancellationToken cancellationToken);
    
    public Task<List<Meditation>> GetLikedMeditationsAsync(Guid userId, CancellationToken cancellationToken);
}