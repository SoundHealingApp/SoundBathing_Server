using Auth.Core.Interfaces;
using Auth.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

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