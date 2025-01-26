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
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public MeditationType MeditationType { get; private set; }
    
    public string? TherapeuticPurpose { get; private set; }

    public double? Rating { get; private set; } // от 1 до 5
    
    public string ImageLink { get; private set; } = string.Empty;
    
    public string AudioLink { get; private set; } = string.Empty;
    
    public double? Frequency { get; private set; }
    
    public List<MeditationFeedback> Feedbacks { get; set; } = [];

    public void SetRating()
    {
        var totalUserRatingsCount = Feedbacks.Count;
        var sumOfUserRatings = Feedbacks.Sum(x => x.Estimate);
        
        Rating = Math.Round((double)sumOfUserRatings / totalUserRatingsCount, 2);
    }
    
    public void SetImageKey(string imageKey) => ImageLink = imageKey;
    
    public void SetAudioKey(string audioKey) => AudioLink = audioKey;

    public void SetTitle(string title) => Title = title.Trim();
    
    public void SetDescription(string description) => Description = description.Trim();
    
    public void SetMeditationType(MeditationType meditationType) => MeditationType = meditationType;
    
    public void SetTherapeuticPurpose(string therapeuticPurpose) => TherapeuticPurpose = therapeuticPurpose.Trim();
    
    public void SetFrequency(double? frequency) => Frequency = frequency;
    
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