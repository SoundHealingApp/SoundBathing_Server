namespace SoundHealing.Core.Models;

public class Permission
{ 
    public Guid Id { get; }
    public string Name { get; }

    public Permission(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
#pragma warning disable CS8618, CS9264
    public Permission() {}
#pragma warning restore CS8618, CS9264
}