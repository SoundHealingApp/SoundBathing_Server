namespace SoundHealing.Core.Models;

public class User
{
    public User(string id, string name, string surname, DateTime birthDate)
    {
        Id =  Guid.Parse(id);
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
    }
    
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string Surname { get; private set; }

    public DateTime BirthDate { get; private set; }

    public List<Meditation> LikedMeditations { get; set; } = [];
    
#pragma warning disable CS8618, CS9264
    public User() {}
#pragma warning restore CS8618, CS9264
}