namespace SoundHealing.Application.Contracts.Dto;

public class UserDto(Guid id, string name, string surname, DateOnly birthDate)
{
    public Guid Id { get; init; } = id;

    public string Name { get; private set; } = name;

    public string Surname { get; private set; } = surname;

    public DateOnly BirthDate { get; private set; } = birthDate;
}