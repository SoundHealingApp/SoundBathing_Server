using SoundHealing.Core.Models;

namespace SoundHealing.Application.Interfaces;

public interface IJwtProvider
{
    public string GenerateToken(UserCredentials userCredentials);
}