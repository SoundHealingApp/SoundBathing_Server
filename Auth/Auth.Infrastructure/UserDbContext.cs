using Auth.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}
    
    public DbSet<UserEntity> Users { get; set; }
}