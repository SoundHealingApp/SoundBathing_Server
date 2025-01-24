namespace SoundHealing.Core.Models;

public class User
{
    public User(string id, string name, string surname, DateOnly birthDate)
    {
        Id =  Guid.Parse(id);
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
    }
    
    public Guid Id { get; init; }

    public string Name { get; private set; }

    public string Surname { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public HashSet<Meditation> LikedMeditations { get; } = [];
    
    public HashSet<Meditation> RecommendedMeditations { get; } = [];

    public void SetLikeToMeditation(Meditation meditation)
    {
        LikedMeditations.Add(meditation);
    }

    public void DeleteLikeFromMeditation(Meditation meditation)
    {
        LikedMeditations.Remove(meditation);
    }

    public void AddRecommendedMeditations(List<Meditation> meditations)
    {
        // Очищаем, так как при каждом добавлении рекоммендаций, старых не должно оставаться.
        RecommendedMeditations.RemoveWhere(x => !meditations.Contains(x));
        
        RecommendedMeditations.UnionWith(meditations); // Гарантируем уникальность.
    }

    public void ChangeName(string name) => Name = name;
    
    public void ChangeSurname(string surname) => Surname = surname;
    
    public void ChangeBirthDate(DateOnly birthDate) => BirthDate = birthDate;
    

#pragma warning disable CS8618, CS9264
    public User() {}
#pragma warning restore CS8618, CS9264
}