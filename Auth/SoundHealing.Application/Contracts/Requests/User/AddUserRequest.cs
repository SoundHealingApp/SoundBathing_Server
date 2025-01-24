namespace SoundHealing.Application.Contracts.Requests.User;

public record AddUserRequest(string UserId, string Name, string Surname, DateOnly BirthDate);