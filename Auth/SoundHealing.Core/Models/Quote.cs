namespace SoundHealing.Core.Models;

public class Quote
{
    public Quote(string text, string author)
    {
        Id = Guid.NewGuid();
        
        Text = text;
        Author = author;
    }

    public Guid Id { get; }
    
    public string Text { get; private set; }
    
    public string Author { get; private set; }

    
    public void ChangeText(string text)
    {
        Text = text.Trim();
    }
    
    public void ChangeAuthor(string author)
    {
        Author = author.Trim();
    }

#pragma warning disable CS8618, CS9264
    public Quote() {}
#pragma warning restore CS8618, CS9264
}