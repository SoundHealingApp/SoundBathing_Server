using SoundHealing.Core.Enums;

namespace SoundHealing.Core.Models;

public class Meditation
{
    public Meditation(
        string title,
        string description,
        MeditationType meditationType,
        string? therapeuticPurpose,
        double? frequency)
    {
        Id = Guid.NewGuid();
        
        Title = title;
        Description = description;
        MeditationType = meditationType;
        TherapeuticPurpose = therapeuticPurpose;
        Frequency = frequency;
    }

    public Guid Id { get; }
    
    public string Title { get; }
    
    public string Description { get;  }
    
    public MeditationType MeditationType { get; }
    
    public string? TherapeuticPurpose { get; }

    public double? Rating { get; } // от 1 до 5
    
    public string ImageLink { get; private set; }
    
    public string AudioLink { get; private set; }
    
    public double? Frequency { get; }
    
    private int SumOfUserRatings { get; set; }
    
    private int TotalUserRatingsCount { get; set; }

    public void SetRatingEstimation(int estimation)
    {
        SumOfUserRatings += estimation;
        TotalUserRatingsCount += 1;
    }

    public double GetRating() => (double)SumOfUserRatings / TotalUserRatingsCount;
    
    public void SetS3Keys(string imageKey, string audioKey)
    {
        ImageLink = imageKey;
        AudioLink = audioKey;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Meditation other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618, CS9264
    public Meditation() {}
#pragma warning restore CS8618, CS9264
}