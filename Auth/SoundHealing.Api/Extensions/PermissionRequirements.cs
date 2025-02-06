using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Extensions;

public class PermissionRequirements : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirements(string permission)
    {
        Permission = permission;
    }
}

public class PermissionRequirementsHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirements>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirements requirement)
    {
        var userId = context.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        
        using var scope = serviceScopeFactory.CreateScope();
        var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
        
        if (userId == null || userRepository == null)
            return;
        
        var permissions = await userRepository.GetPermissionsByUserIdAsync(userId.Value);
        
        if (permissions.Any(x => x.Name == requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}