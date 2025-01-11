namespace Auth.Application.Contracts.Requests;

public record UserRegisterRequest(string Email, string Password);