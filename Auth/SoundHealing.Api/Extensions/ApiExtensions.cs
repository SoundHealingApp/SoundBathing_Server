using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SoundHealing.Core;
using SoundHealing.Infrastructure.Options;

namespace SoundHealing.Extensions;

public static class ApiExtensions
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };
            });
        
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementsHandler>();
        var authorizationBuilder = services.AddAuthorizationBuilder();
        
        foreach (var policy in PermissionsConstants.AdminPermissions.Concat(PermissionsConstants.UserPermissions))
        {
            authorizationBuilder.AddPolicy(policy, builder =>
                builder.Requirements.Add(new PermissionRequirements(policy)));
        }
    }
}