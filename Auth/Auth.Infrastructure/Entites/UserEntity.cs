namespace Auth.Infrastructure.Entites;

public class UserEntity(string userName, string passwordHash)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserName { get; set; } = userName;

    public string PasswordHash { get; set; } = passwordHash;
}