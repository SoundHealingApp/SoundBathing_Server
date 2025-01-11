using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserRepository(UserDbContext userDbContext) : IUserRepository
{
    public async Task<Guid> AddAsync(User user)
    {
        var userEntity = new UserEntity(user.Email, user.PasswordHash);

        await userDbContext.Users.AddAsync(userEntity);
        await userDbContext.SaveChangesAsync();
        
        return userEntity.Id;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var userEntity = await userDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
        
        if (userEntity == null)
            return null;
        
        var user = User.Create(userEntity.Email, userEntity.PasswordHash);

        return user;
    }
}