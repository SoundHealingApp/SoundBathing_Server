using Auth.Core.Models;

namespace Auth.Core.Interfaces;

public interface IUserRepository
{
    public Task<Guid> AddAsync(User user);
    
    public Task<User?> GetByEmailAsync(string email);
}