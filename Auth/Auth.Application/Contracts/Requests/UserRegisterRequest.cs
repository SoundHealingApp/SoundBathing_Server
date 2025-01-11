namespace Auth.Application.Contracts.Requests;

public record UserRegisterRequest(string UserName, string Password);