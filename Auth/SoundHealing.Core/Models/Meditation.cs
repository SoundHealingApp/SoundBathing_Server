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

    public Guid Id { get; }
    
    public string Title { get; }
    
    public string Description { get;  }
    
    public MeditationType MeditationType { get; }
    
    public string TherapeuticPurpose { get; } // TODO: Потом можно сделать enum

    public double? Rating { get; } // от 1 до 5
    
    public string ImageLink { get; }
    
    public string VideoLink { get; }
    
    public double Frequency { get; }
    
    private int SumOfUserRatings { get; set; }
    
    private int TotalUserRatingsCount { get; set; }

    public void SetRatingEstimation(int estimation)
    {
        SumOfUserRatings += estimation;
        TotalUserRatingsCount += 1;
    }

    public double GetRating() => (double)SumOfUserRatings / TotalUserRatingsCount;
}