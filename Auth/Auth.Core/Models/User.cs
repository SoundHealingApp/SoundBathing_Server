namespace Auth.Core.Models;

public class User
{
    private User(string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public Guid Id { get; }
    
    public string Email { get; }
    
    public string PasswordHash { get; }

    public static User Create(string email, string passwordHash)
    {
        var user = new User(email, passwordHash);
        return user;
    }
}