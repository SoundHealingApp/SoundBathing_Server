namespace Auth.Application.Contracts.Requests;

public record LoginRequest(string Email, string Password);