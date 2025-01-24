namespace SoundHealing.Application.Contracts.Requests.Auth;

public record ChangeCredentialsRequest(string? Email, string? Password);