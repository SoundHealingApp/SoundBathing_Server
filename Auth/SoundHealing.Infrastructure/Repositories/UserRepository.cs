using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

// TODO: посмотреть как лучше возвращать ошибки
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
        var user = await userDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        
        return user;
    }
    
    public async Task<bool> SetLikeToMeditationAsync(Guid userId, Guid meditationId, CancellationToken cancellationToken)
    {
        var user = await userDbContext.Users
            .Include(user => user.LikedMeditations)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        var meditation = await userDbContext.Meditations
            .FirstOrDefaultAsync(x => x.Id == meditationId, cancellationToken);

        if (user == null || meditation == null)
            return false;
        
        user.SetLikeToMeditation(meditation);
        
        await userDbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> DeleteLikeFromMeditationAsync(Guid userId, Guid meditationId, CancellationToken cancellationToken)
    {
        var user = await userDbContext.Users
            .Include(user => user.LikedMeditations)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        
        if (user == null)
            return false;
        
        user.DeleteLikeFromMeditation(meditationId);
        
        await userDbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<List<Meditation>> GetLikedMeditationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        // TODO: проверять что пользователь существует
        var user = await userDbContext.Users
            .Include(x => x.LikedMeditations)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        return user == null ? [] : user.LikedMeditations.ToList();
    }
}