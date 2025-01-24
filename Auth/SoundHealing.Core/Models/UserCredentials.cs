namespace SoundHealing.Core.Models;

// TODO: проверка Email
public class UserCredentials
{
    public UserCredentials(string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public Guid Id { get; set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set;}

    public static UserCredentials Create(string email, string passwordHash)
    {
        var user = new UserCredentials(email, passwordHash);
        return user;
    }

    public void ChangeEmail(string email)
    {
        Email = email;
    }

    public void ChangePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
    
#pragma warning disable CS8618, CS9264
    public UserCredentials() { }
#pragma warning restore CS8618, CS9264
}