namespace Auth.Infrastructure.Entites;

public class UserEntity(string email, string passwordHash)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = email;

    public string PasswordHash { get; set; } = passwordHash;
}