namespace SoundHealing.Application.Interfaces;

public interface IPasswordHasher
{
    public string Generate(string password);
    
    bool Verify(string password, string hash);
}