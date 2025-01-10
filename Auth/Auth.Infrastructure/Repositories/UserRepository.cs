using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserRepository(UserDbContext userDbContext) : IUserRepository
{
    public async Task<Guid> AddAsync(User user)
    {
        // TODO: сделать маппинг
        var userEntity = new UserEntity(user.UserName, user.PasswordHash);

        await userDbContext.Users.AddAsync(userEntity);
        await userDbContext.SaveChangesAsync();
        
        return userEntity.Id;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        var userEntity = await userDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == userName);
        
        if (userEntity == null)
            return null;
        
        var user = User.Create(userEntity.UserName, userEntity.PasswordHash);

        return user;
    }
}