using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}
    
    public DbSet<UserCredentials> UsersCredentials { get; set; }
    
    public DbSet<User> Users { get; set; }
}