using Auth.Core.Models;

namespace Auth.Core.Interfaces;

public interface IUserCredentialsRepository
{
    public Task<Guid> AddAsync(UserCredentials userCredentials);
    
    public Task<UserCredentials?> GetByEmailAsync(string email);
}