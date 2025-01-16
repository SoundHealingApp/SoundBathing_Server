using SoundHealing.Core.Enums;

namespace SoundHealing.Core.Models;

public class Meditation
{
    public Meditation(
        string title,
        string description,
        MeditationType meditationType,
        string therapeuticPurpose,
        string imageLink,
        string videoLink,
        double frequency)
    {
        Id = Guid.NewGuid();
        
        Title = title;
        Description = description;
        MeditationType = meditationType;
        TherapeuticPurpose = therapeuticPurpose;
        ImageLink = imageLink;
        VideoLink = videoLink;
        Frequency = frequency;
    }

    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public MeditationType MeditationType { get; set; }
    
    public string TherapeuticPurpose { get; set; } // TODO: Потом можно сделать enum
    
    public double? Rating { get; set; } // от 1 до 5
    
    public string ImageLink { get; set; }
    
    public string VideoLink { get; set; }
    
    public double Frequency { get; set; }

    public List<User> LikedUsers { get; set; } = [];
    
    private int SumOfUserRatings { get; set; }
    
    private int TotalUserRatingsCount { get; set; }

    public void SetRatingEstimation(int estimation)
    {
        SumOfUserRatings += estimation;
        TotalUserRatingsCount += 1;
    }

    public double GetRating() => (double)SumOfUserRatings / TotalUserRatingsCount;
}