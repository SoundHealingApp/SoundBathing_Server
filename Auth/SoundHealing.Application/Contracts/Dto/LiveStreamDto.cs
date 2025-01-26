namespace SoundHealing.Application.Contracts.Dto;

public class LiveStreamDto
{
    public LiveStreamDto(
        Guid id,
        string title,
        string description,
        string? therapeuticPurpose,
        DateTime startDateTime,
        string youTubeUrl)
    {
        Id = id;
        Title = title;
        Description = description;
        TherapeuticPurpose = therapeuticPurpose;
        FormattedStartDateTime = startDateTime.ToString("dd-MM-yyyy HH:mm");;
        YouTubeUrl = youTubeUrl;
    }

    public Guid Id { get;}
    
    public string Title { get; }
    
    public string Description { get; }
    
    public string? TherapeuticPurpose { get; private set; }
    
    public string FormattedStartDateTime { get;  }
    
    public string YouTubeUrl { get; private set; }
}