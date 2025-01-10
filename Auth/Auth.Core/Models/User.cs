namespace Auth.Core.Models;

public class User
{
    private User(string userName, string passwordHash)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        PasswordHash = passwordHash;
    }
    
    public Guid Id { get; }
    
    public string UserName { get; }
    
    public string PasswordHash { get; }

    public static User Create(string userName, string passwordHash)
    {
        // TODO: validate
        var user = new User(userName, passwordHash);
        return user;
    }
}