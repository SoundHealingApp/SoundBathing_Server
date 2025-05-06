namespace SoundHealing.Core.Interfaces;

public interface ISecretProvider
{
    string GetSecret(string name);
}