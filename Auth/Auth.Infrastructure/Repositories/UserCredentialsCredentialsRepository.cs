using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserCredentialsCredentialsRepository(UserDbContext userDbContext) : IUserCredentialsRepository
{
    public async Task<Guid> AddAsync(UserCredentials userCredentials)
    {
        var userEntity = new UserCredentialsEntity(userCredentials.Email, userCredentials.PasswordHash);

        await userDbContext.UsersCredentials.AddAsync(userEntity);
        await userDbContext.SaveChangesAsync();
        
        return userEntity.Id;
    }

    public async Task<UserCredentials?> GetByEmailAsync(string email)
    {
        var userEntity = await userDbContext.UsersCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
        
        if (userEntity == null)
            return null;
        
        var user = UserCredentials.Create(userEntity.Email, userEntity.PasswordHash);

        return user;
    }
}