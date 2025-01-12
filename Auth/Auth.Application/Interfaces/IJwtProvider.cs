using Auth.Core.Models;

namespace Auth.Application.Interfaces;

public interface IJwtProvider
{
    public string GenerateToken(UserCredentials userCredentials);
}