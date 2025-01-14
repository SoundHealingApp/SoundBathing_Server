using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IUserRepository
{
    public Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
    
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}