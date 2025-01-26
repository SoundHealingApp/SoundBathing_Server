using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class UserCredentialsRepository(AppDbContext appDbContext) : IUserCredentialsRepository
{
    public async Task<Guid> AddAsync(UserCredentials userCredentials)
    {
        await appDbContext.UsersCredentials.AddAsync(userCredentials);
        await appDbContext.SaveChangesAsync();
        
        return userCredentials.Id;
    }

    public async Task<UserCredentials?> GetByEmailAsync(string email)
    {
        var userCredentials = await appDbContext.UsersCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

        return userCredentials;
    }

    public async Task<UserCredentials?> GetByUserIdAsync(Guid userId)
    {
        var userCredentials = await appDbContext.UsersCredentials
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        return userCredentials;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => 
        appDbContext.SaveChangesAsync(cancellationToken);
}