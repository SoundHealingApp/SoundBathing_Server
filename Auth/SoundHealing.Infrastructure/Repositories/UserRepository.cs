using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class UserRepository(UserDbContext userDbContext) : IUserRepository
{
    public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken)
    {
        await userDbContext.AddAsync(user, cancellationToken);
        await userDbContext.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = await userDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (userEntity == null)
            return null;

        var user = new User(userEntity.Id.ToString(), userEntity.Name, userEntity.Surname, userEntity.BirthDate);
        
        return user;
    }
}