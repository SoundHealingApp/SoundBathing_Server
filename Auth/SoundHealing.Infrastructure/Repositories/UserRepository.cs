using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken)
    {
        await appDbContext.AddAsync(user, cancellationToken);
        
        return user.Id;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
            .Include(x => x.MeditationFeedbacks)
            .Include(x => x.LikedMeditations)
            .Include(x => x.RecommendedMeditations)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        
        return user;
    }

    public async Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
         await appDbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsyncWithoutIncludes(Guid userId, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        
        return user;
    }

    public async Task<ICollection<Permission>> GetPermissionsByUserIdAsync(string userId)
    {
        var user = await appDbContext.Users
            .Include(user => user.Permissions)
            .FirstOrDefaultAsync(x => x.Id.ToString() == userId);
        
        if (user == null)
            throw new InvalidOperationException($"User with id {userId} not found");
        
        return user.Permissions;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => appDbContext.SaveChangesAsync(cancellationToken);
}