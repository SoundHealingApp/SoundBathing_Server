namespace Auth.Application.Contracts.Requests;

public record AddUserRequest(string UserCredentialsId, string Name, string Surname, DateTime BirthDate);