using Azure.Security.KeyVault.Secrets;
using SoundHealing.Core.Interfaces;

namespace SoundHealing.Infrastructure.Providers;

public class AzureKeyVaultSecretProvider(SecretClient client) : ISecretProvider
{
    public string GetSecret(string name)
    {
        var secret = client.GetSecret(name);
        return secret.Value.Value;
    }
}