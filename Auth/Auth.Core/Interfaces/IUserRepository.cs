using Auth.Core.Models;

namespace Auth.Core.Interfaces;

public interface IUserRepository
{
    public Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
    
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}