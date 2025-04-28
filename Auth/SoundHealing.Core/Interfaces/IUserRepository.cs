using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IUserRepository
{
    public Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
    
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    public Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    public Task<User?> GetByIdAsyncWithoutIncludes(Guid userId, CancellationToken cancellationToken);

    public Task<ICollection<Permission>> GetPermissionsByUserIdAsync(string userId);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}