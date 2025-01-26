namespace SoundHealing.Core.Models;

public class LiveStream
{
    public LiveStream(string title, string description, string? therapeuticPurpose, DateTime startDateTime, string youTubeUrl)
    {
        Id = Guid.NewGuid();
        
        Title = title;
        Description = description;
        TherapeuticPurpose = therapeuticPurpose;
        StartDateTime = startDateTime;
        YouTubeUrl = youTubeUrl;
    }

    public Guid Id { get; }
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public string? TherapeuticPurpose { get; private set; }
    
    public DateTime StartDateTime { get; private set; }
    
    public string YouTubeUrl { get; private set; }

    public void ChangeTitle(string newTitle)
    {
        Title = newTitle;
    }
    
    public void ChangeDescription(string description)
    {
        Description = description;
    }
    
    public void ChangeTherapeuticPurpose(string therapeuticPurpose)
    {
        TherapeuticPurpose = therapeuticPurpose;
    }
    
    public void ChangeStartDateTime(DateTime startDateTime)
    {
        StartDateTime = startDateTime;
    }
    
    public void ChangeYouTubeUrl(string youTubeUrl)
    {
        YouTubeUrl = youTubeUrl;
    }
    
#pragma warning disable CS8618, CS9264
    public LiveStream() {}
#pragma warning restore CS8618, CS9264
}
