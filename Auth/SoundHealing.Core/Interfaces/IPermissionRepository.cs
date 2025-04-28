using SoundHealing.Core.Models;

namespace SoundHealing.Core.Interfaces;

public interface IPermissionRepository
{
    public Task<List<Permission>> GetUserPermissionsAsync(CancellationToken cancellationToken);

    public Task<bool> CheckUserHasPermissionAsync(
        Guid userId, 
        string permissionName,
        CancellationToken cancellationToken);
}