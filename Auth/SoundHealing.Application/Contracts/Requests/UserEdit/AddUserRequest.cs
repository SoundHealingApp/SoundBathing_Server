namespace SoundHealing.Application.Contracts.Requests.UserEdit;

public record AddUserRequest(string UserCredentialsId, string Name, string Surname, DateTime BirthDate);