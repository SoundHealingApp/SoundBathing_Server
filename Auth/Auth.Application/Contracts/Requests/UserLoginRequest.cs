namespace Auth.Application.Contracts.Requests;

public record UserLoginRequest(string Email, string Password);