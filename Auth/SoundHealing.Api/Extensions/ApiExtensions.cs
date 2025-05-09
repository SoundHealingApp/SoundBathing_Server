using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.IdentityModel.Tokens;
using SoundHealing.Core;
using SoundHealing.Infrastructure.Options;

namespace SoundHealing.Extensions;

public static class ApiExtensions
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration, 
        IWebHostEnvironment environment,
        SecretClient secretClient)
    {
        var jwtSecretKey = "";
        
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        if (environment.IsDevelopment())
        {
            jwtSecretKey = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!.SecretKey;
        } 
        else if (environment.IsProduction())
        {          
            jwtSecretKey = secretClient.GetSecret("JwtOptions").Value.Value!;
        }

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
                        Encoding.UTF8.GetBytes(jwtSecretKey))
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