using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SoundHealing.Application.Interfaces;
using SoundHealing.Core.Interfaces;
using SoundHealing.Core.Models;
using SoundHealing.Infrastructure.Options;

namespace SoundHealing.Infrastructure.Providers;

public class JwtProvider(IOptions<JwtOptions> options, ISecretProvider secretProvider) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken(UserCredentials userCredentials)
    {
        Claim[] claims = [
            new (ClaimTypes.NameIdentifier, userCredentials.Id.ToString()),
        ];
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretProvider.GetSecret("JwtOptions"))),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddDays(_options.ExpiresDays));
        
        var tokenValue= new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenValue;
    }
}