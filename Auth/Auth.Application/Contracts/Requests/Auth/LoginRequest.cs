namespace Auth.Application.Contracts.Requests.Auth;

public record LoginRequest(string Email, string Password);