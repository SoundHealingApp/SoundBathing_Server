namespace SoundHealing.Application.Contracts.Requests.User;

public record ChangeUserDataRequest(string? Name, string? Surname, DateOnly? BirthDate);