namespace Auth.Application.Contracts.Requests;

public record UserLoginRequest(string UserName, string Password);