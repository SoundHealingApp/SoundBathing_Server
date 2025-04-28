using Microsoft.EntityFrameworkCore;
using SoundHealing.Core;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Repositories;

public class PermissionRepository(AppDbContext appContext): IPermissionRepository
{
    public async Task<List<Permission>> GetUserPermissionsAsync(CancellationToken cancellationToken)
    {
        return await appContext.Permission.Where(x => PermissionsConstants.UserPermissions.Contains(x.Name))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> CheckUserHasPermissionAsync(
        Guid userId, 
        string permissionName,
        CancellationToken cancellationToken)
    {
        return await appContext.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Permissions)
            .AnyAsync(p => p.Name == permissionName, cancellationToken);
    }
}