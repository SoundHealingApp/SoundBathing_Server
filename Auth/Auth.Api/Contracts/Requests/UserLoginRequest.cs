namespace Auth.Contracts.Requests;

public record UserLoginRequest(string UserName, string Password);