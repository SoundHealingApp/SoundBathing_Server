namespace SoundHealing.Application.Contracts.Requests.UserEdit;

public record AddUserRequest(string UserId, string Name, string Surname, DateOnly BirthDate);