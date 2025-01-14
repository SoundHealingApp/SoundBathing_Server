using Auth.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class UserCredentialsRepository(UserDbContext userDbContext) : IUserCredentialsRepository
{
    public async Task<Guid> AddAsync(UserCredentials userCredentials)
    {
        await userDbContext.UsersCredentials.AddAsync(userCredentials);
        await userDbContext.SaveChangesAsync();
        
        return userCredentials.Id;
    }

    public async Task<UserCredentials?> GetByEmailAsync(string email)
    {
        var userCredentials = await userDbContext.UsersCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

        return userCredentials;
    }
}