namespace Auth.Core.Models;

public class UserCredentials
{
    private UserCredentials(string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public Guid Id { get; }
    
    public string Email { get; }
    
    public string PasswordHash { get; }

    public static UserCredentials Create(string email, string passwordHash)
    {
        var user = new UserCredentials(email, passwordHash);
        return user;
    }
}