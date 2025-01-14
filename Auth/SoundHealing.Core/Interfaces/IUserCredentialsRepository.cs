using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IUserCredentialsRepository
{
    public Task<Guid> AddAsync(UserCredentials userCredentials);
    
    public Task<UserCredentials?> GetByEmailAsync(string email);
}